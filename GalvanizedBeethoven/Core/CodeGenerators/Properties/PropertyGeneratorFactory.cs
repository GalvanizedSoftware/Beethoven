using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  internal class PropertyGeneratorFactory
  {
    private readonly MemberInfo memberInfo;
    private readonly IDefinition[] definitions;
    private readonly GeneratorContext generatorContext;

    public PropertyGeneratorFactory(GeneratorContext generatorContext, IEnumerable<IDefinition> definitions)
    {
      this.generatorContext = generatorContext;
      memberInfo = this.generatorContext.MemberInfo;
      this.definitions = definitions
        .Where(definition => definition.CanGenerate(memberInfo))
        .ToArray();
    }

    internal ICodeGenerator Create() => definitions.Length switch
    {
      0 => new PropertyNotImplementedGenerator(),
      1 => GetSingleGenerator(definitions.Single()),
      _ => GetMultiGenerator()
    };

    private ICodeGenerator GetSingleGenerator(IDefinition definition) =>
      definition switch
      {
        PropertyDefinition propertyDefinition => new PropertyGenerator(propertyDefinition),
        DefaultProperty defaultProperty => defaultProperty,
        IDefinition otherDefinition => otherDefinition.GetGenerator(generatorContext),
        _ => throw new MissingMethodException()
      };

    private ICodeGenerator GetMultiGenerator()
    {
      IDefinition[] specificPropertyDefinitions = definitions
        .Where(definition => definition.SortOrder <= 1)
        .ToArray();
      if (specificPropertyDefinitions.Length == 1)
        return GetSingleGenerator(specificPropertyDefinitions.Single());
      throw new MissingMethodException($"Multiple implementation of {memberInfo.Name} found");
    }
  }
}