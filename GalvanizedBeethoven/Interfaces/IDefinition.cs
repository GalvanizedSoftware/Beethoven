using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Interfaces
{
  public interface IDefinition
  {
    int SortOrder { get; }
    bool CanGenerate(MemberInfo memberInfo);
    ICodeGenerator GetGenerator(GeneratorContext generatorContext);
  }
}