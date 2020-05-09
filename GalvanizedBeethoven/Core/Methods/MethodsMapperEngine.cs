using System;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class MethodsMapperEngine
  {
    private readonly MethodInfo[] interfaceMethods;

    public MethodsMapperEngine(Type mainType)
    {
      interfaceMethods = mainType.GetAllMethodsAndInherited().ToArray();
    }

    public IEnumerable<MappedMethod> GenerateMappings(object baseObject, Type baseType) =>
      baseType
        .GetAllTypes()
        .SelectMany(type => type.GetNotSpecialMethods())
        .Intersect(interfaceMethods, new ExactMethodComparer())
        .Select(methodInfo => new MappedMethod(methodInfo, baseObject))
        .ToArray();
  }
}