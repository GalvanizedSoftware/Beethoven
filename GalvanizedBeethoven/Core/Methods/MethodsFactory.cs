using System.Collections;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Core.Interceptors;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class MethodsFactory : IEnumerable<InterceptorMap>
  {
    private readonly IEnumerable<Method> methods;

    public MethodsFactory(IEnumerable<Method> methods)
    {
      this.methods = methods;
    }

    public IEnumerator<InterceptorMap> GetEnumerator()
    {
      foreach (Method method in methods)
        yield return new InterceptorMap(method.Name, new MethodInterceptor(method));
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}