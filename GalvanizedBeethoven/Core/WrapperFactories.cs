using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Events;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class WrapperFactories<T> : IEnumerable<InterceptorMap>
  {
    private readonly InterceptorMap[] interceptorMaps;

    public WrapperFactories(List<object> wrappers, EventInvokers eventInvokers)
    {
      interceptorMaps =
        new PropertiesFactory(wrappers.OfType<Property>())
        .Concat(new MethodsFactory(wrappers.OfType<Method>()))
        .Concat(new EventsFactory<T>(eventInvokers))
        .SelectMany(provider => provider.GetInterceptorMaps<T>())
        .Where(map => map.Item1 != null)
        .ToArray();
    }

    public IEnumerator<InterceptorMap> GetEnumerator() => 
      ((IEnumerable<InterceptorMap>)interceptorMaps).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}