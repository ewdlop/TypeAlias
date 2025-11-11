using AliasDirective.Shared;
using Azure.AI.Translation.Text;
using Azure.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using NuGet.Packaging;
using System.Collections.Concurrent;
using System.Reflection;

namespace AliasDirective.Console;

public class Program
{
    protected static string[] _targetLanguages = [];
    protected static TextTranslationClient? client = null;

    public static async Task Main(string[] args)
    {
        System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        {
            //https://learn.microsoft.com/en-us/nuget/api/catalog-resource
        }

        System.Console.WriteLine("\n=== Dependency Context ===\n");
        Task.Run(DependencyUtilities.PrintDependencyContext).Wait();

        //string endpoint = "<Text Translator Custom Endpoint>";
        //DefaultAzureCredential credential = new DefaultAzureCredential(true);
        //client = new TextTranslationClient(new Uri(endpoint));

        //Azure.Response<GetSupportedLanguagesResult> result = await client.GetSupportedLanguagesAsync();

        ////https://en.wikipedia.org/wiki/IETF_language_tag
        //_targetLanguages = [.. result.Value.Translation.Keys];

        var packageArchiveReaderBag = await PackageUtilities.DownloadPackagesAndAsync([("Newtonsoft.Json", "13.0.3")]);

        while(packageArchiveReaderBag.TryTake(out PackageArchiveReader? packageArchiveReader))
        {
            if (packageArchiveReader == null) continue;
            Assembly loadedAssembly = await PackageUtilities.ExtraTransformLoadAssembly(packageArchiveReader);
            Dictionary<string, string?> typeDictionary = TypeTraversalUtilities.PrintPosteriorTraversal(loadedAssembly)
                .DistinctBy(type => $"{type.FullName?.Split(type.Name).First()}{type.Name}")
                .ToDictionary(type => type.Name, type => type.FullName);

            foreach (KeyValuePair<string, string?> kvp in typeDictionary)
            {
                string alias = $"using {kvp.Key} = {kvp.Value};";
                System.Console.WriteLine(alias);
                //How to check a compilation is successful?
            }

            //await foreach (TranslatedTextItem translatedTextItem in TranslationUtilities.TranslateAsync(_targetLanguages, typeDictionary.Keys, sourceLanguage: "en", client: client))
            //{
            //    foreach (TranslationText translationText in translatedTextItem.Translations)
            //    {
            //        System.Console.WriteLine($"using {translationText.Text} = {typeDictionary[translatedTextItem.SourceText.Text]}");
            //        //check compilation
            //    }
            //}
        }
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
                if (load)
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