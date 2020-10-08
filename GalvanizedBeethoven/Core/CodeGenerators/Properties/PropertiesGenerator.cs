using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  class PropertiesGenerator : ICodeGenerator
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
        .Select(propertyInfo => generatorContext.CreateLocal(propertyInfo, CodeType.Properties))
        .SelectMany(localContext => new PropertyGeneratorFactory(localContext, definitions)
          .Create()
          .Generate(localContext));
  }
}
