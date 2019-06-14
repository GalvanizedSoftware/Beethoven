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
  }
}
