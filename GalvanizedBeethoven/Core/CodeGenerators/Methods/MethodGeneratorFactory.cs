using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class MethodGeneratorFactory
  {
    private readonly IDefinition[] definitions;

    public MethodGeneratorFactory(IEnumerable<IDefinition> definitions)
    {
      this.definitions = definitions.ToArray();
    }

    internal ICodeGenerator Create(MethodInfo methodInfo, int index)
    {
      IDefinition[] matchingDefinitions = definitions
        .Where(definition => definition.CanGenerate(methodInfo))
        .ToArray();
      return matchingDefinitions.Length switch
      {
        0 => new MethodNotImplementedGenerator(methodInfo),
        _ => new MethodGenerator(methodInfo, index),
      };
    }
  }
}
