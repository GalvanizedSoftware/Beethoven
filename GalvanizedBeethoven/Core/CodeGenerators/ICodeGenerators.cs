using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators
{
  internal interface ICodeGenerators : IEnumerable<ICodeGenerator>
  {
    IEnumerable<(CodeType, string)?> Generate(GeneratorContext generatorContext);
  }
}