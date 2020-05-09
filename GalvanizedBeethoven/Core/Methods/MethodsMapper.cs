using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class MethodsMapper<T> : IEnumerable<MethodDefinition>, IEnumerable<IDefinition>
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

    public IEnumerator<MethodDefinition> GetEnumerator() => methods.AsEnumerable().GetEnumerator();

    IEnumerator<IDefinition> IEnumerable<IDefinition>.GetEnumerator() =>
      methods.OfType<IDefinition>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}