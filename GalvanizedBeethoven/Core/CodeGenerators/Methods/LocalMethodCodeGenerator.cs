using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class LocalMethodCodeGenerator : ICodeGenerator
  {
    private readonly GeneratorContext generatorContext;
    private readonly ICodeGenerator innerCodeGenerator;

    public LocalMethodCodeGenerator(
      GeneratorContext generatorContext, ICodeGenerator innerCodeGenerator, MethodInfo methodInfo, int? index)
    {
      this.generatorContext = generatorContext.CreateLocal(methodInfo, index);
      this.innerCodeGenerator = innerCodeGenerator;
    }

    public IEnumerable<(CodeType, string)?> Generate() =>
      innerCodeGenerator.Generate();
  }
}