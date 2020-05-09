using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  internal static class DictionaryExtensions
  {
    internal static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) =>
      dictionary.TryGetValue(key, out TValue value) ? value : default;
  }
}
