using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces
{
  internal interface ICodeGenerators
  {
    IEnumerable<ICodeGenerator> GetGenerators(GeneratorContext generatorContext);
  }
}