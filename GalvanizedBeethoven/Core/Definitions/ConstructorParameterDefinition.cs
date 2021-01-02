using System;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
  internal class ConstructorFieldsWrapperDefinition : DefaultDefinition
  {
    private readonly Func<GeneratorContext, ICodeGenerator> generatorFunc;

    public static IDefinition Create(Func<GeneratorContext, ICodeGenerator> generatorFunc) =>
      new ConstructorFieldsWrapperDefinition(generatorFunc);

    private ConstructorFieldsWrapperDefinition(Func<GeneratorContext, ICodeGenerator> generatorFunc)
    {
      this.generatorFunc = generatorFunc;
    }

    public override ICodeGenerator GetGenerator(GeneratorContext generatorContext) => 
      generatorFunc(generatorContext);
  }
}
