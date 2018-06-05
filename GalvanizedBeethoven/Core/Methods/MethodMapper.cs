using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class MethodMapper<T> : IEnumerable<Method>
  {
    private readonly Method[] methods = new Method[0];

    public MethodMapper(object baseObject)
    {
      if (baseObject == null)
        return;
      MethodInfo[] interfaceMethods = typeof(T).GetMethods();
      IEnumerable<MethodInfo> implementationMethods = baseObject
        .GetType()
        .GetMethods(Constants.ResolveFlags)
        .Where(info => !info.IsSpecialName);
      IEnumerable<MethodInfo> methodInfos = implementationMethods
        .Intersect(interfaceMethods, new EquivalentMethodComparer())
        .ToArray();
      methods = methodInfos
        .Select(methodInfo => (Method)new MethodsWithInstance(methodInfo) { Instance = baseObject })
        .ToArray();
    }

    public IEnumerator<Method> GetEnumerator()
    {
      return methods.AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}