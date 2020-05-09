using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  class PropertiesGenerator
  {
    private readonly PropertyInfo[] propertyInfos;
    private IEnumerable<IDefinition> definitions;

    public PropertiesGenerator(MemberInfo[] membersInfos, IEnumerable<IDefinition> definitions)
    {
      propertyInfos = membersInfos
        .OfType<PropertyInfo>()
        .ToArray();
      this.definitions = definitions;
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext) =>
      propertyInfos
        .SelectMany(propertyInfo => new PropertyGeneratorFactory(propertyInfo, definitions)
          .Create()
          .Generate(generatorContext.CreateLocal(propertyInfo)));
  }
}
