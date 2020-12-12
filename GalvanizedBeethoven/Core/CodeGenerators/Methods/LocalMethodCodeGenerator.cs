using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class LocalMethodCodeGenerator : ICodeGenerator
  {
    private readonly ICodeGenerator innerCodeGenerator;

    public LocalMethodCodeGenerator(ICodeGenerator innerCodeGenerator)
    {
      this.innerCodeGenerator = innerCodeGenerator;
    }

    public IEnumerable<(CodeType, string)?> Generate() =>
      innerCodeGenerator.Generate();
  }
}