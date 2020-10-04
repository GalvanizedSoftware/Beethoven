using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class MethodGeneratorFactory
  {
    private readonly MethodInfo methodInfo;
    private readonly MethodDefinition[] definitions;

    public MethodGeneratorFactory(MethodInfo methodInfo, IEnumerable<MethodDefinition> definitions)
    {
      this.methodInfo = methodInfo;
      this.definitions = definitions
          .Where(definition => definition.CanGenerate(methodInfo))
          .ToArray();
    }

    internal ICodeGenerator Create()
    {
      return definitions.Length switch
      {
        0 => new MethodNotImplementedGenerator(),
        1 => GetSingleGenerator(definitions.Single()),
        _ => GetMultiGenerator()
      };
    }

    private static ICodeGenerator GetSingleGenerator(IDefinition definition)
    {
      return definition switch
      {
        MethodDefinition methodDefinition => new MethodGenerator(methodDefinition),
        //DefaultMethod defaultMethod => defaultMethod,
        _ => throw new MissingMethodException()
      };
    }

    private ICodeGenerator GetMultiGenerator()
    {
      IDefinition[] specificDefinitions = definitions
        .Where(definition => definition.SortOrder <= 1)
        .ToArray();
      if (specificDefinitions.Length == 1)
        return GetSingleGenerator(specificDefinitions.Single());
      if (methodInfo.IsGenericMethod)
        return new MethodGenerator(new GenericMethodDefinition(methodInfo, definitions));
      throw new MissingMethodException($"Multiple implementation of {methodInfo.Name} found");
    }
  }
}