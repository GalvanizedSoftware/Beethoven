using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using System;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Constructor;
using GalvanizedSoftware.Beethoven.Generic;

namespace GalvanizedSoftware.Beethoven.Core.Properties.Instances
{
  public class ConstructorInitializedInstance : DefaultDefinition
  {
    private readonly ICodeGenerator generator;

    public ConstructorInitializedInstance(string name, Type type)
    {
      generator = new PropertyInitializedGenerator(name, type);
    }

    public override ICodeGenerator GetGenerator(GeneratorContext _) => generator;
  }
}
