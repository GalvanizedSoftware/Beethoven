using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal sealed class CodeGeneratorList : IEnumerable<ICodeGenerator>, ICodeGenerator
  {
    private readonly ICodeGenerator[] codeGenerators;

    public CodeGeneratorList(params ICodeGenerator[] codeGenerators)
    {
      this.codeGenerators = codeGenerators;
    }

    public IEnumerable<(CodeType, string)?> Generate() =>
      codeGenerators
        .SelectMany(generator => generator.Generate());

    public IEnumerator<ICodeGenerator> GetEnumerator() => 
      (IEnumerator<ICodeGenerator>) codeGenerators.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => 
      GetEnumerator();
  }
}