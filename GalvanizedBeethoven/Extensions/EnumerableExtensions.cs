using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  public static class EnumerableExtensions
  {
    public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> enumerable)
    {
      T[] array = enumerable.ToArray();
      int length = array.Length;
      for (int i = 0; i < length - 1; i++)
        yield return array[i];
    }

    public static bool AllAndNonEmpty<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = null)
    {
      if (source == null)
        return false;
      predicate = predicate ?? (item => item.Equals(true));
      bool result = false;
      foreach (TSource element in source)
      {
        if (!predicate(element))
          return false;
        result = true;
      }
      return result;
    }

    public static IEnumerable<T> ExceptIndex<T>(this IEnumerable<T> enumerable, int skipIndex) =>
      enumerable.Where((item, i) => i != skipIndex);

    internal static string ToString(this IEnumerable<char> chars) =>
      new string(chars.ToArray());

    internal static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
      if (collection == null)
        return;
      foreach (T item in collection)
        action(item);
    }

    internal static IDefinition[] GetAllDefinitions(this IEnumerable<object> collection) =>
      collection
        .SelectMany(GetAllDefinitions)
        .Distinct()
        .OrderBy(definition => definition.SortOrder)
        .ToArray();

    private static IEnumerable<IDefinition> GetAllDefinitions(object part) =>
      part switch
      {
        IDefinitions definitions => definitions.GetDefinitions(),
        IDefinition definition => new[] { definition },
        _ => Enumerable.Empty<IDefinition>()
      };

    internal static (CodeType, string)[] GenerateCode(this IEnumerable<IDefinition> definitions, GeneratorContext generatorContext) =>
      definitions
        .GetGenerators(generatorContext)
        .SelectMany(generator => generator.Generate(generatorContext))
        .SkipNull()
        .ToArray();

    internal static IEnumerable<ICodeGenerator> GetGenerators(
      this IEnumerable<IDefinition> definitions, GeneratorContext generatorContext) =>
      definitions
        .Where(definition => definition.CanGenerate(generatorContext.MemberInfo))
        .Select(definition => definition.GetGenerator(generatorContext))
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
