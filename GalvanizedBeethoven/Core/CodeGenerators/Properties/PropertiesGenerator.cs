using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  class PropertiesGenerator : ICodeGenerator
  {
    private readonly PropertyInfo[] propertyInfos;
    private readonly IEnumerable<IDefinition> definitions;

    public PropertiesGenerator(MemberInfo[] membersInfos, IEnumerable<IDefinition> definitions)
    {
      propertyInfos = membersInfos
        .OfType<PropertyInfo>()
        .ToArray();
      this.definitions = definitions;
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext) =>
      propertyInfos
        .Select(propertyInfo => generatorContext.CreateLocal(propertyInfo))
        .SelectMany(localContext => new PropertyGeneratorFactory(localContext, definitions)
          .Create()
          .Generate(localContext));
  }
}
