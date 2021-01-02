using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class MethodGeneratorFactory
  {
    private readonly GeneratorContext generatorContext;
    private readonly MethodInfo methodInfo;
    private readonly MethodDefinition[] definitions;

    public MethodGeneratorFactory(GeneratorContext generatorContext, MethodInfo methodInfo, int? index, IEnumerable<MethodDefinition> definitions)
    {
      this.generatorContext = generatorContext.CreateLocal(methodInfo, index);
      this.methodInfo = methodInfo;
      this.definitions = definitions
          .Where(definition => definition.CanGenerate(methodInfo))
          .ToArray();
    }

    internal ICodeGenerator Create()
    {
      return definitions.Length switch
      {
        0 => new MethodNotImplementedGenerator(generatorContext),
        1 => GetSingleGenerator(definitions.Single()),
        _ => GetMultiGenerator()
      };
    }

    private ICodeGenerator GetSingleGenerator(IDefinition definition)
    {
      return definition switch
      {
        MethodDefinition methodDefinition => new MethodGenerator(generatorContext, methodDefinition),
        _ => throw new MissingMethodException()
      };
    }

    private ICodeGenerator GetMultiGenerator()
    {
      MethodDefinition[] specificDefinitions = definitions
        .Where(definition => definition.SortOrder <= 1)
        .ToArray();
      if (specificDefinitions.Length == 1)
        return GetSingleGenerator(specificDefinitions.Single());
      return methodInfo.IsGenericMethod ?
        new MethodGenerator(generatorContext, new GenericMethodDefinition(methodInfo, definitions)) :
        throw new MissingMethodException($"Multiple implementation of {methodInfo.Name} found");
    }
  }
}