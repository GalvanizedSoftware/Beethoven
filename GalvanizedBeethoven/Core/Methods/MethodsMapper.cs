using System;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class MethodsMapper<T> : IEnumerable<Method>
  {
    private static readonly MethodInfo[] interfaceMethods =
      typeof(T).GetAllMethodsAndInherited().ToArray();
    private readonly Method[] methods;

    public MethodsMapper(object baseObject)
    {
      IParameter parameter = (baseObject as DefinitionImport)?.Parameter;
      Type baseType = parameter?.Type ?? baseObject?.GetType();
      Func<MethodInfo, Method> methodCreator = parameter != null ? (Func<MethodInfo, Method>)
        (methodInfo => new ParameterMappedMethod(methodInfo, parameter)) :
        methodInfo => new MappedMethod(methodInfo, baseObject);
      methods = baseType
        .GetAllTypes()
        .SelectMany(type => type.GetNotSpecialMethods())
        .Intersect(interfaceMethods, new ExactMethodComparer())
        .Select(methodCreator).ToArray();
    }

    public IEnumerator<Method> GetEnumerator() => methods.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}