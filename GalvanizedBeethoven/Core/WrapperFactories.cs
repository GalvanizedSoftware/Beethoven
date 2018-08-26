using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class WrapperFactories : IEnumerable<InterceptorMap>
  {
    private readonly InterceptorMap[] interceptorMaps;

    public WrapperFactories(List<object> wrappers)
    {
      interceptorMaps =
        new PropertiesFactory(wrappers.OfType<Property>())
        .Concat(new MethodsFactory(wrappers.OfType<Method>()))
        .ToArray();
    }

    public IEnumerator<InterceptorMap> GetEnumerator()
    {
      return ((IEnumerable<InterceptorMap>)interceptorMaps).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}