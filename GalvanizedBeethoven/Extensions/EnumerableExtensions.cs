using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  public static class EnumerableExtensions
  {
    public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> parameters)
    {
      T[] array = parameters.ToArray();
      int length = array.Length;
      for (int i = 0; i < length - 1; i++)
        yield return array[i];
    }
  }
}
