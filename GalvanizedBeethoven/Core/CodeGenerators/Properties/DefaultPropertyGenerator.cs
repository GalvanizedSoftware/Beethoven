using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Definitions;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  public class DefaultPropertyGenerator
  {
    private readonly DefaultPropertyDefinitions propertyDefinitions;

    internal DefaultPropertyGenerator(DefaultPropertyDefinitions propertyDefinitions)
    {
      this.propertyDefinitions = propertyDefinitions;
    }

    internal ICodeGenerator GetGenerator(GeneratorContext generatorContext) =>
      propertyDefinitions.Create(generatorContext?.MemberInfo as PropertyInfo)
        .GetGenerator(generatorContext);
  }
}