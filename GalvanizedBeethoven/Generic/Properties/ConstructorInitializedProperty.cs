﻿using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using System;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Constructor;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class ConstructorInitializedProperty : DefaultDefinition
  {
    private readonly ICodeGenerator generator;

    public ConstructorInitializedProperty(string name, Type type)
    {
      generator = new ParameterFieldGenerator(type, name, $"parameter{name}");
    }

    public override ICodeGenerator GetGenerator(GeneratorContext _) => generator;
  }
}
