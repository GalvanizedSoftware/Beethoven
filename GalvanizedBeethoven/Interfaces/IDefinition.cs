using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Interfaces
{
  public interface IDefinition
  {
    int SortOrder { get; }
    bool CanGenerate(MemberInfo memberInfo);
    ICodeGenerator GetGenerator(GeneratorContext generatorContext);
    IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo);
  }
}