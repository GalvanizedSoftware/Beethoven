using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces
{
  public interface ICodeGenerator
  {
    IEnumerable<(CodeType, string)?> Generate();
  }
}
