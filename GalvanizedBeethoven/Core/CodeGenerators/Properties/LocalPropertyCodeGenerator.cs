using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class LocalPropertyCodeGenerator : ICodeGenerator
  {
    private readonly ICodeGenerator innerCodeGenerator;
    private readonly PropertyInfo propertyInfo;

    public LocalPropertyCodeGenerator(ICodeGenerator innerCodeGenerator, PropertyInfo propertyInfo)
    {
      this.innerCodeGenerator = innerCodeGenerator;
      this.propertyInfo = propertyInfo;
    }

    public IEnumerable<(CodeType, string)?> Generate(GeneratorContext generatorContext) =>
      innerCodeGenerator.Generate(generatorContext.CreateLocal(propertyInfo));
  }
}