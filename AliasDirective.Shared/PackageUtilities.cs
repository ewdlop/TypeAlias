using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using System.Collections.Concurrent;
using System.Reflection;

namespace AliasDirective.Shared;

public static class PackageUtilities
{
    public static async Task<IProducerConsumerCollection<PackageArchiveReader>> DownloadPackagesAndAsync((string packageId, string version)[] packages)
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
                Console.WriteLine("✅.nupkg downloaded");
                PackageArchiveReader packageArchiveReader = new PackageArchiveReader(stream);
                NuspecReader nuspec = await packageArchiveReader.GetNuspecReaderAsync(token);
                nuspec.GetDependencyGroups().ToList().ForEach(dg =>
                {
                    Console.WriteLine($"Target Framework: {dg.TargetFramework.GetShortFolderName()}");
                    dg.Packages.ToList().ForEach(dep =>
                    {
                        Console.WriteLine($"  Dependency: {dep.Id}, Version Range: {dep.VersionRange}");
                    });
                });
                nuspec.GetPackageTypes().ToList().ForEach(pt =>
                {
                    Console.WriteLine($"Package Type: {pt.Name}, Version: {pt.Version}");
                });
                _ = packageArchiveReader.CopyNupkgAsync($"{package.packageId.ToLower()}.{package.version}.nupkg", token);
                stream.Position = 0;
                packageArchiveReaderBag.Add(packageArchiveReader);
            }
        });
        return packageArchiveReaderBag;
    }

    public static async Task<Assembly> ExtraTransformLoadAssembly(PackageArchiveReader packageArchiveReader, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("\n=== Framework Reduction ===\n");
        // 目前執行環境的 TFM
        NuGetFolderPath[] nuGetFolderPaths = [NuGetFolderPath.NuGetHome];
        string[] nuGetFolderPathStrings = [".net8.0"];

        NuGetFramework nuGetFramework = NuGetFramework.ParseFolder(nuGetFolderPathStrings.Last());

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
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine($"Compatible Item for {reducedNuGetFramework.GetShortFolderName()}: {item}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    
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
}