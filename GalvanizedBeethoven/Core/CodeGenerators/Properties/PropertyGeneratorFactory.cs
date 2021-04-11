using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  internal class PropertyGeneratorFactory
  {
    private readonly PropertyInfo propertyInfo;
    private readonly IDefinition[] definitions;

    public PropertyGeneratorFactory(PropertyInfo propertyInfo, IEnumerable<IDefinition> definitions)
    {
      this.propertyInfo = propertyInfo;
      this.definitions = definitions
        .Where(definition => definition.CanGenerate(propertyInfo))
        .ToArray();
    }

    internal ICodeGenerator Create() => definitions.Length switch
    {
      0 => new PropertyNotImplementedGenerator(propertyInfo),
      1 => GetSingleGenerator(definitions.Single()),
      _ => GetMultiGenerator()
    };

    private ICodeGenerator GetSingleGenerator(IDefinition definition) =>
      definition.GetGenerator(propertyInfo);

    private ICodeGenerator GetMultiGenerator()
    {
      IDefinition[] specificPropertyDefinitions = definitions
        .Where(definition => definition.SortOrder <= 1)
        .ToArray();
      if (specificPropertyDefinitions.Length == 1)
        return GetSingleGenerator(specificPropertyDefinitions.Single());
      throw new MissingMethodException($"Multiple implementation of {propertyInfo.Name} found");
    }
  }
}