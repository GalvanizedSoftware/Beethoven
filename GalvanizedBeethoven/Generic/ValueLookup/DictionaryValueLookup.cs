using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Generic.ValueLookup
{
  public class DictionaryValueLookup : IValueLookup
  {
    private readonly Dictionary<string, object> defaultValues;

    public DictionaryValueLookup(Dictionary<string, object> defaultValues)
    {
      this.defaultValues = defaultValues;
    }

    public IEnumerable<T> Lookup<T>(string name)
    {
      if (!defaultValues.TryGetValue(name, out object objectValue))
        yield break;
      if (!(objectValue is T))
        yield break;
      yield return (T)objectValue;
    }
  }
}
