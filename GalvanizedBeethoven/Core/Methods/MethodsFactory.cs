using System.Collections;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Core.Interceptors;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class MethodsFactory : IEnumerable<IInterceptorProvider>
  {
    private readonly IEnumerable<Method> methods;

    public MethodsFactory(IEnumerable<Method> methods)
    {
      this.methods = methods;
    }

    public IEnumerator<IInterceptorProvider> GetEnumerator() => 
      methods.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}