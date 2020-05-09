using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Generic.Parameters;
using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Fields
{
  internal class GeneratorWrapperDefinition : IDefinition, IParameter
  {
    private readonly ICodeGenerator generator;
    private readonly Func<MemberInfo, bool> checkerFunc;

    public static IDefinition Create<T>(ICodeGenerator<T> generator) where T : MemberInfo =>
      new GeneratorWrapperDefinition(typeof(T), generator, (memberInfo) => memberInfo is T);

    private GeneratorWrapperDefinition(Type type, ICodeGenerator generator, Func<MemberInfo, bool> checkerFunc)
    {
      this.generator = generator;
      this.checkerFunc = checkerFunc;
      Type = type;
    }

    public int SortOrder => 1;

    public Type Type { get; }

    public string Name => null;

    public bool CanGenerate(MemberInfo memberInfo) => checkerFunc(memberInfo);

    public ICodeGenerator GetGenerator() => generator;
  }
}
