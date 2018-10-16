using GalvanizedSoftware.Beethoven.Generic.Methods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class MultiMethodMapper<T> : IEnumerable<Method>
  {
    private readonly Method[] methods = new Method[0];

    public MultiMethodMapper(object baseObject)
    {
      if (baseObject == null)
        return;
      MethodInfo[] interfaceMethods = typeof(T).GetAllMethodsAndInherited().ToArray();
      IEnumerable<MethodInfo> implementationMethods = baseObject
        .GetType()
        .GetMethods(Constants.ResolveFlags)
        .Where(info => !info.IsSpecialName);
      IEnumerable<MethodInfo> methodInfos = implementationMethods
        .Intersect(interfaceMethods, new EquivalentMethodComparer())
        .ToArray();
      methods = methodInfos
        .Select(methodInfo => (Method)new MappedMethod(methodInfo) { Instance = baseObject })
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