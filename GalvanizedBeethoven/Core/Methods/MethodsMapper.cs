using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class MethodsMapper<T> : IEnumerable<MethodDefinition>, IDefinitions
  {

    private static readonly MethodsMapperEngine methodsMapperEngine = new(typeof(T));
    private readonly MappedMethod[] methods;

    public MethodsMapper(object baseObject)
    {
      Type baseType = baseObject?.GetType();
      methods = methodsMapperEngine
        .GenerateMappings(baseObject , baseType)
        .ToArray();
    }

    public IEnumerable<IDefinition> GetDefinitions<TInterface>() where TInterface : class => methods;

    public IEnumerator<MethodDefinition> GetEnumerator() => methods.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}