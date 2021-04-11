using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
  internal class ConstructorFieldsWrapperDefinition : DefaultDefinition
  {
    private readonly Func<MemberInfo, ICodeGenerator> generatorFunc;

    public static IDefinition Create(Func<MemberInfo, ICodeGenerator> generatorFunc) =>
      new ConstructorFieldsWrapperDefinition(generatorFunc);

    private ConstructorFieldsWrapperDefinition(Func<MemberInfo, ICodeGenerator> generatorFunc)
    {
      this.generatorFunc = generatorFunc;
    }

    public override ICodeGenerator GetGenerator(MemberInfo memberInfo) => 
      generatorFunc(memberInfo);
  }
}
