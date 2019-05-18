using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Generic;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class InstanceMap
  {
    private readonly Dictionary<object, object> dictionary;

    public InstanceMap(IEnumerable<object> wrappers)
    {
      dictionary = wrappers.OfType<Parameter>().ToDictionary(
        parameter => parameter.Definition,
        parameter => parameter.Create());
    }

    public object GetLocal(object interceptor)
    {
      dictionary.TryGetValue(interceptor, out object localInstance);
      return localInstance;
    }
  }
}