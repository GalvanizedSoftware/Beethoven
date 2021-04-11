using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  public static class EnumerableExtensions
  {
	  public static bool AllAndNonEmpty<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = null)
    {
      if (source == null)
        return false;
      predicate ??= item => item.Equals(true);
      bool result = false;
      foreach (TSource element in source)
      {
        if (!predicate(element))
          return false;
        result = true;
      }
      return result;
    }

	  internal static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
      if (collection == null)
        return;
      foreach (T item in collection)
        action(item);
    }

    internal static void ForEach<T1, T2>(this IEnumerable<(T1, T2)> collection, Action<T1, T2> action)
    {
      if (collection == null)
        return;
      foreach ((T1, T2) item in collection)
        action(item.Item1, item.Item2);
    }

    internal static (CodeType, string)[] GenerateCode(this IEnumerable<IDefinition> definitions, MemberInfo memberInfo) =>
      definitions
        .GetGenerators(memberInfo)
        .SelectMany(generator => generator.Generate())
        .SkipNull()
        .ToArray();

    internal static IEnumerable<ICodeGenerator> GetGenerators(this IEnumerable<IDefinition> definitions, 
	    MemberInfo memberInfo) =>
      definitions
        .Where(definition => definition.CanGenerate(memberInfo))
        .Select(definition => definition.GetGenerator(memberInfo))
        .SkipNull();

    public static IEnumerable<T> SkipNull<T>(this IEnumerable<T> enumerable) where T : class
    {
      if (enumerable == null)
        yield break;
      foreach (T item in enumerable)
        if (item != null)
          yield return item;
    }

    public static IEnumerable<T> SkipNull<T>(this IEnumerable<T?> enumerable) where T : struct
    {
      if (enumerable == null)
        yield break;
      foreach (T? item in enumerable.Where(item => item.HasValue))
        yield return item.Value;
    }
  }
}
