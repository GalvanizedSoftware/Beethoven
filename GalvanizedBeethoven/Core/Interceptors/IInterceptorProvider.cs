using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal interface IInterceptorProvider
  {
    IEnumerable<InterceptorMap> GetInterceptorMaps<T>();
  }
}