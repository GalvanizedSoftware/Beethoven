using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class LocalMethodCodeGenerator : ICodeGenerator
  {
    private readonly ICodeGenerator innerCodeGenerator;
    private readonly MethodInfo methodInfo;
    private readonly int? index;

    public LocalMethodCodeGenerator(ICodeGenerator innerCodeGenerator, MethodInfo methodInfo, int? index)
    {
      this.innerCodeGenerator = innerCodeGenerator;
      this.methodInfo = methodInfo;
      this.index = index;
    }

    public IEnumerable<(CodeType, string)?> Generate(GeneratorContext generatorContext) =>
      innerCodeGenerator.Generate(generatorContext.CreateLocal(methodInfo, index));
  }
}