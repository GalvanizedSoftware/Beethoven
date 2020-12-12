using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  internal class LocalPropertyCodeGenerator : ICodeGenerator
  {
    private readonly ICodeGenerator innerCodeGenerator;

    public LocalPropertyCodeGenerator(ICodeGenerator innerCodeGenerator)
    {
      this.innerCodeGenerator = innerCodeGenerator;
    }

    public IEnumerable<(CodeType, string)?> Generate() =>
      innerCodeGenerator.Generate();
  }
}