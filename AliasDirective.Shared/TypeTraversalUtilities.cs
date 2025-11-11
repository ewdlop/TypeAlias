using System.Reflection;

namespace AliasDirective.Shared;

public static class TypeTraversalUtilities
{
    public static IEnumerable<Type> PrintPosteriorTraversal(Assembly assembly)
    {
        foreach (Type type in assembly.GetTypes().SelectMany(t => PostOrder(t)))
        {
            Console.WriteLine($"using {type.Name} = {type.FullName};");
            yield return type;
        }
        foreach (Type type in PostOrder(assembly.GetType()))
        {
            Console.WriteLine($"using {type.Name} = {type.FullName};");
            yield return type;
        }
    }

    public static IEnumerable<Type> InOrder(Type node, HashSet<Type>? visited = null)
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

    public static IEnumerable<Type> PostOrder(Type node, HashSet<Type>? visited = null)
    {
        visited ??= new HashSet<Type>();
        if (node is null || !visited.Add(node)) yield break;
        foreach (Type val in node.GetProperties().SelectMany(child => PostOrder(child.PropertyType, visited)))
        {
            yield return val;
        }
        yield return node;
    }
}