using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class MethodsMapper<T> : IEnumerable<MethodDefinition>, IDefinitions
  {

    private static readonly MethodsMapperEngine methodsMapperEngine = new MethodsMapperEngine(typeof(T));
    private readonly MethodDefinition[] methods;

    public MethodsMapper(object baseObject)
    {
      Type baseType = baseObject?.GetType();
      methods = methodsMapperEngine
        .GenerateMappings(baseObject , baseType)
        .ToArray();
    }

    public IEnumerable<IDefinition> GetDefinitions() => methods.OfType<IDefinition>();

    public IEnumerator<MethodDefinition> GetEnumerator() => methods.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}