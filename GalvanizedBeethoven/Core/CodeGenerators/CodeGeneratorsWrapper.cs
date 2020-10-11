using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven
{
  internal class CodeGeneratorsWrapper : ICodeGenerator
  {
    private ICodeGenerators codeGenerators;

    public CodeGeneratorsWrapper(ICodeGenerators codeGenerators)
    {
      this.codeGenerators = codeGenerators;
    }

    public IEnumerable<(CodeType, string)?> Generate(GeneratorContext generatorContext) =>
      codeGenerators
        .GetGenerators(generatorContext)
        .SelectMany(codeGenerator => codeGenerator.Generate(generatorContext));
  }
}