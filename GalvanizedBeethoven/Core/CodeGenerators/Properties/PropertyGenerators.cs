using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  class PropertyGenerators : ICodeGenerators
  {
    private readonly PropertyInfo[] propertyInfos;
    private readonly IEnumerable<IDefinition> definitions;

    public PropertyGenerators(MemberInfo[] membersInfos, IEnumerable<IDefinition> definitions)
    {
      propertyInfos = membersInfos
        .OfType<PropertyInfo>()
        .ToArray();
      this.definitions = definitions;
    }

    public IEnumerable<ICodeGenerator> GetGenerators(GeneratorContext generatorContext) =>
      propertyInfos
        .Select(
          propertyInfo => new PropertyGeneratorFactory(generatorContext, propertyInfo, definitions)
            .Create()
            .WrapLocal(generatorContext, propertyInfo));
  }
}
