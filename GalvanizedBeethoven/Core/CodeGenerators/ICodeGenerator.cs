using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators
{
  public interface ICodeGenerator
  {
    IEnumerable<string> Generate(GeneratorContext generatorContext);
  }
}
