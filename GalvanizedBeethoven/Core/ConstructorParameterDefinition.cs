using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Fields
{
  internal class ContructorFieldsWrapperDefinition : IDefinition
  {
    private readonly ICodeGenerator generator;

    public static IDefinition Create(ICodeGenerator generator) =>
      new ContructorFieldsWrapperDefinition(generator);

    private ContructorFieldsWrapperDefinition(ICodeGenerator generator)
    {
      this.generator = generator;
    }

    public int SortOrder => 1;

    public bool CanGenerate(MemberInfo memberInfo) => 
      memberInfo is ConstructorInfo || memberInfo is FieldInfo;

    public ICodeGenerator GetGenerator(GeneratorContext _) => generator;
  }
}
