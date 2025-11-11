using Microsoft.Extensions.DependencyModel;
using Microsoft.TemplateEngine.Utils;

namespace AliasDirective.Shared;

public static class DependencyUtilities
{
    public static void PrintDependencyContext()
    {
        DependencyContext? dependencyContext = DependencyContext.Default;

        if (dependencyContext is not null)
        {
            Dictionary<string, HashSet<string>> dependenciesMap = new Dictionary<string, HashSet<string>>();

            foreach (RuntimeLibrary runtimeLibrary in dependencyContext.RuntimeLibraries)
            {
                Console.WriteLine($"Library: {runtimeLibrary.Name}, Version: {runtimeLibrary.Version}");
                HashSet<string> dependencies = new HashSet<string>();
                foreach (Dependency dependency in runtimeLibrary.Dependencies)
                {
                    dependencies.Add(dependency.Name);
                }
                dependenciesMap[runtimeLibrary.Name] = dependencies;
            }

            if (TryGetTopologicalSorter(dependenciesMap, out IReadOnlyList<string> sortedLibraries))
            {
                Console.WriteLine("\nTopologically Sorted Libraries:");
                foreach (string library in sortedLibraries)
                {
                    Console.WriteLine(library);
                }
            }
            else
            {
                Console.WriteLine("A dependency loop detected!");
            }
        }
    }

    public static bool TryGetTopologicalSorter<T>(Dictionary<T, HashSet<T>> dependenciesMap, out IReadOnlyList<T> values)
    {
        DirectedGraph<T> directedGraph = new DirectedGraph<T>(dependenciesMap);
        return directedGraph.TryGetTopologicalSort(out values);
    }
}