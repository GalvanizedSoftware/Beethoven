using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Fields
{
  internal class GeneratorWrapperDefinition : IDefinition
  {
    private readonly ICodeGenerator generator;
    private readonly Func<MemberInfo, bool> checkerFunc;

    public static IDefinition Create<T>(ICodeGenerator<T> generator) where T : MemberInfo =>
      new GeneratorWrapperDefinition(generator, (memberInfo) => memberInfo is T);

    private GeneratorWrapperDefinition(ICodeGenerator generator, Func<MemberInfo, bool> checkerFunc)
    {
      this.generator = generator;
      this.checkerFunc = checkerFunc;
    }

    public int SortOrder => 1;

    public bool CanGenerate(MemberInfo memberInfo) => checkerFunc(memberInfo);

    public ICodeGenerator GetGenerator(GeneratorContext _) => generator;
  }
}
