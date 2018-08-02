using System.Linq;
using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  class DefaultImplementation
  {
    private readonly Dictionary<string, object> values = new Dictionary<string, object>();

    public void DelegatedSetter<T>(string name, T value)
    {
      values[name] = value;
    }

    internal object[] GetObjects()
    {
      return values.Values.ToArray();
    }
  }
}
