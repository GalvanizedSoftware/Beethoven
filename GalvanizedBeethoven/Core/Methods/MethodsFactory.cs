using System.Collections;
using System.Collections.Generic;

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
        yield return new InterceptorMap(method.Name, method);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}