using Azure.AI.Translation.Text;
using Azure.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Extensions.DependencyModel;
using Microsoft.TemplateEngine.Utils;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Reflection;

namespace AliasDirective.Console;

public class Program
{
    protected static string[] _targetLanguages = [];
    protected static TextTranslationClient client = null;
    public static async Task Main(string[] args)
    {
        System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        {
            //https://learn.microsoft.com/en-us/nuget/api/catalog-resource
        }

        System.Console.WriteLine("\n=== Dependency Context ===\n");
        Task.Run(PrintDependencyContext).Wait();

        //string endpoint = "<Text Translator Custom Endpoint>";
        //DefaultAzureCredential credential = new DefaultAzureCredential(true);
        //client = new TextTranslationClient(new Uri(endpoint));

        //Azure.Response<GetSupportedLanguagesResult> result = await client.GetSupportedLanguagesAsync();

        ////https://en.wikipedia.org/wiki/IETF_language_tag
        //_targetLanguages = [.. result.Value.Translation.Keys];

        var packageArchiveReaderBag = await DownloadPackagesAndAsync([("Newtonsoft.Json", "13.0.3")]);

        while(packageArchiveReaderBag.TryTake(out PackageArchiveReader? packageArchiveReader))
        {
            if (packageArchiveReader == null) continue;
            Assembly loadedAssembly = await ExtraTransformLoadAssembly(packageArchiveReader);
            Dictionary<string, string?> typeDictionary = PrintPosteriorTraversal(loadedAssembly)
                .DistinctBy(type => type.FullName)
                .ToDictionary(type => type.Name, type => type.FullName); //counting duplicate type names, name collision

            foreach (KeyValuePair<string, string?> kvp in typeDictionary)
            {
                string alias = $"using {kvp.Key} = {kvp.Value};";
                System.Console.WriteLine(alias);
                //How to check a compilation is successful?
            }

            //await foreach (TranslatedTextItem translatedTextItem in TranslateAsync(_targetLanguages, typeDictonary.Keys, sourceLanguage: "en", client: client))
            //{
            //    foreach (TranslationText translationText in translatedTextItem.Translations)
            //    {
            //        System.Console.WriteLine($"using {translationText.Text} = {typeDictonary[translatedTextItem.SourceText.Text]}");
            //        //check compilation
            //    }
            //}
        }
    }

    static async IAsyncEnumerable<TranslatedTextItem> TranslateAsync(IEnumerable<string> targetLanguages, IEnumerable<string> texts, string? sourceLanguage = null, CancellationToken cancellationToken = default, TextTranslationClient client = null)
    {
        Azure.Response<IReadOnlyList<TranslatedTextItem>>? result = await client.TranslateAsync(targetLanguages, texts, Guid.NewGuid(), sourceLanguage: sourceLanguage, cancellationToken: cancellationToken);
        foreach (TranslatedTextItem item in result.Value)
        {
            yield return item;
        }
    }

    static async Task<IProducerConsumerCollection<PackageArchiveReader>> DownloadPackagesAndAsync((string packageId, string version)[] packages)
    {
        SourceRepository source = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
        FindPackageByIdResource? packageByIdResource = await source.GetResourceAsync<FindPackageByIdResource>();
        using SourceCacheContext sourceCacheContext = new SourceCacheContext();
        ConcurrentBag<PackageArchiveReader> packageArchiveReaderBag = new ConcurrentBag<PackageArchiveReader>();
        await Parallel.ForEachAsync(packages, async (package, token) =>
        {
            var stream = new MemoryStream();
            if (await packageByIdResource.CopyNupkgToStreamAsync(package.packageId, new NuGet.Versioning.NuGetVersion(package.version),
                                                    stream, sourceCacheContext, NullLogger.Instance, token))
            {
                System.Console.WriteLine("✅.nupkg downloaded");
                PackageArchiveReader packageArchiveReader = new PackageArchiveReader(stream);
                NuspecReader nuspec = await packageArchiveReader.GetNuspecReaderAsync(token);
                nuspec.GetDependencyGroups().ToList().ForEach(dg =>
                {
                    System.Console.WriteLine($"Target Framework: {dg.TargetFramework.GetShortFolderName()}");
                    dg.Packages.ToList().ForEach(dep =>
                    {
                        System.Console.WriteLine($"  Dependency: {dep.Id}, Version Range: {dep.VersionRange}");
                    });
                });
                nuspec.GetPackageTypes().ToList().ForEach(pt =>
                {
                    System.Console.WriteLine($"Package Type: {pt.Name}, Version: {pt.Version}");
                });
                _ = packageArchiveReader.CopyNupkgAsync($"{package.packageId.ToLower()}.{package.version}.nupkg", token);
                stream.Position = 0;
                packageArchiveReaderBag.Add(packageArchiveReader);
            }
        });
        return packageArchiveReaderBag;
    }

    static async Task<Assembly> ExtraTransformLoadAssembly(PackageArchiveReader packageArchiveReader, CancellationToken cancellationToken = default)
    {
        System.Console.WriteLine("\n=== Framework Reduction ===\n");
        // 目前執行環境的 TFM
        NuGetFolderPath[] nuGetFolderPaths = [NuGetFolderPath.NuGetHome];
        string[] nuGetFolderPathStrings = [".net8.0"];

        NuGetFramework nuGetFramework = NuGetFramework.ParseFolder(nuGetFolderPathStrings.Last()); // 簡化示範；實務上可用更健全偵測

        FrameworkReducer frameworkReducer = new FrameworkReducer();

        var frameworkSpecificGroups = await packageArchiveReader.GetLibItemsAsync(cancellationToken);

        NuGetFramework? reducedNuGetFramework = frameworkReducer.GetNearest(nuGetFramework, frameworkSpecificGroups.Select(g => g.TargetFramework)) ?? throw new Exception("No compatible Target Framework found.");

        Assembly loadedAssembly = Assembly.GetCallingAssembly();
        frameworkSpecificGroups.Select(g => g.Items)
            .Where((items, index) => frameworkSpecificGroups.ElementAt(index).TargetFramework.Equals(reducedNuGetFramework)).ToList()
            .ForEach(items =>
            {
                items.ToList().ForEach(item =>
                {
                    System.Console.BackgroundColor = System.ConsoleColor.DarkBlue;
                    System.Console.WriteLine($"Compatible Item for {reducedNuGetFramework.GetShortFolderName()}: {item}");
                    System.Console.BackgroundColor = System.ConsoleColor.Black;
                    // 這裡可以進一步處理 item，例如解壓縮 DLL 並載入等
                    // miminal common subtraction
                    string outputDir = Path.Combine("nuget-reduced");
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                    if (item.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    {
                        string outputPath = Path.Combine(outputDir, Path.GetFileName(item));
                        packageArchiveReader.ExtractFile(item, outputPath, NullLogger.Instance);


                        loadedAssembly = Assembly.LoadFrom(outputPath);
                    }
                });
            });
        return loadedAssembly;
    }

    static void PrintDependencyContext()
    {
        DependencyContext? dependencyContext = DependencyContext.Default;

        if (dependencyContext is not null)
        {
            Dictionary<string, HashSet<string>> dependenciesMap = new Dictionary<string, HashSet<string>>();

            foreach (RuntimeLibrary runtimeLibrary in dependencyContext.RuntimeLibraries)
            {
                System.Console.WriteLine($"Library: {runtimeLibrary.Name}, Version: {runtimeLibrary.Version}");
                HashSet<string> dependncies = new HashSet<string>();
                foreach (Dependency dependency in runtimeLibrary.Dependencies)
                {
                    dependncies.Add(dependency.Name);
                }
                dependenciesMap[runtimeLibrary.Name] = dependncies;
            }

            if (TryGetTopologicalSorter(dependenciesMap, out IReadOnlyList<string> sortedLibraries))
            {
                System.Console.WriteLine("\nTopologically Sorted Libraries:");
                foreach (string library in sortedLibraries)
                {
                    System.Console.WriteLine(library);
                }
            }
            else
            {
                System.Console.WriteLine("A dependency loop detected!");
            }
        }
    }

#if false
    {
      //foreach (Type type in (AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
            //    .SelectMany(a => a.GetTypes())
            //    .Select(t => InOrder(t))))
            //{
            //    //string alias = type.GetCustomAttributes(typeof(System.Runtime.CompilerServices.TypeForwardedToAttribute), false)
            //    //    .OfType<System.Runtime.CompilerServices.TypeForwardedToAttribute>()
            //    //    .FirstOrDefault()?.Destination.FullName ?? type.Name;

            //    System.Console.WriteLine($"using {type.Name} = {type.FullName};");
            //}
    }
#endif

    static IEnumerable<Type> PrintPosteriorTraversal(Assembly assembly)
    {
        // Example of Post-Order Traversal of Types and their Properties
        foreach (Type type in assembly.GetTypes().SelectMany(t => PostOrder(t)))
        {
            System.Console.WriteLine($"using {type.Name} = {type.FullName};");
            yield return type;
        }
        foreach (Type type in PostOrder(assembly.GetType()))
        {
            System.Console.WriteLine($"using {type.Name} = {type.FullName};");
            yield return type;
        }
    }

    /// <summary>
    /// In-order Traversal: Left -> Root(mid) -> Right
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    static IEnumerable<Type> InOrder(Type node, HashSet<Type>? visited = null)
    {
        visited ??= new HashSet<Type>();
        if (node is null || !visited.Add(node)) yield break;

        int mid = node.GetProperties().Length / 2;
        foreach (Type? val in Enumerable.Range(0, mid).SelectMany(i => InOrder(node.GetProperties()[i].PropertyType, visited)))
            yield return val;
        yield return node;
        foreach (Type? val in Enumerable.Range(mid, node.GetProperties().Length - mid).SelectMany(i => InOrder(node.GetProperties()[i].PropertyType, visited)))
            yield return val;
    }

    /// <summary>
    /// Post-order Traversal: Children -> Root
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    static IEnumerable<Type> PostOrder(Type node, HashSet<Type>? visited = null)
    {
        visited ??= new HashSet<Type>();
        if (node is null || !visited.Add(node)) yield break;
        foreach (Type val in node.GetProperties().SelectMany(child => PostOrder(child.PropertyType, visited)))
        {
            yield return val;
        }
        yield return node;
    }

    /// <summary>
    /// Converts a camelCase string to a python_case. 
    /// Author:
    /// </summary>
    /// <param name="input">The camelCase string to convert.</param>
    /// <returns>The converted snake_case string.</returns>
    static string CamelToSnake(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        var stringBuilder = new System.Text.StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (char.IsUpper(c))
            {
                if (i > 0)
                {
                    stringBuilder.Append('_');
                }
                stringBuilder.Append(char.ToLower(c));
            }
            else
            {
                stringBuilder.Append(c);
            }
        }
        return stringBuilder.ToString();
    }

    static bool TryGetTopologicalSorter<T>(Dictionary<T, HashSet<T>> dependenciesMap, out IReadOnlyList<T> values)
    {
        DirectedGraph<T> directedGraph = new DirectedGraph<T>(dependenciesMap);
        return directedGraph.TryGetTopologicalSort(out values);
    }

    static EmitResult Compile(string code, MetadataReference[] metadataReference, string outPutPath)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
        string assemblyName = Path.GetRandomFileName();
        MetadataReference[] references = metadataReference;

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        return compilation.Emit(outPutPath);
    }

    static IList<MetadataReference> GetMetadataReferences(Assembly assembly, bool load = false)
    {
        IList<MetadataReference> references = new List<MetadataReference>()
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(assembly.Location)
        };

        foreach (AssemblyName referencedAssembly in assembly.GetReferencedAssemblies())
        {
            try
            {
                if(load)
                {
                    Assembly loadedAssembly = Assembly.Load(referencedAssembly);
                    references.Add(MetadataReference.CreateFromFile(loadedAssembly.Location));
                }
            }
            catch
            {
                // Skip assemblies that can't be loaded
            }
        }

        return references;
    }
}