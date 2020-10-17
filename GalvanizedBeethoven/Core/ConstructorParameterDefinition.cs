using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Fields
{
  internal class ContructorFieldsWrapperDefinition : IDefinition
  {
    private readonly Func<GeneratorContext, ICodeGenerator> generatorFunc;

    public static IDefinition Create(Func<GeneratorContext, ICodeGenerator> generatorFunc) =>
      new ContructorFieldsWrapperDefinition(generatorFunc);

    private ContructorFieldsWrapperDefinition(Func<GeneratorContext, ICodeGenerator> generatorFunc)
    {
      this.generatorFunc = generatorFunc;
    }

    public int SortOrder => 1;

    public bool CanGenerate(MemberInfo memberInfo) =>
      memberInfo is null;

    public ICodeGenerator GetGenerator(GeneratorContext generatorContext) => 
      generatorFunc(generatorContext);
  }
}
