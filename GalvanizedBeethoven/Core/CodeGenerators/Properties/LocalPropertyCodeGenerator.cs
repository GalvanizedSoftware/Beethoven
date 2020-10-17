using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class LocalPropertyCodeGenerator : ICodeGenerator
  {
    private readonly GeneratorContext generatorContext;
    private readonly ICodeGenerator innerCodeGenerator;
    private readonly PropertyInfo propertyInfo;

    public LocalPropertyCodeGenerator(GeneratorContext generatorContext, ICodeGenerator innerCodeGenerator, PropertyInfo propertyInfo)
    {
      this.generatorContext = generatorContext.CreateLocal(propertyInfo);
      this.innerCodeGenerator = innerCodeGenerator;
      this.propertyInfo = propertyInfo;
    }

    public IEnumerable<(CodeType, string)?> Generate() =>
      innerCodeGenerator.Generate();
  }
}