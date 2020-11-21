﻿using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Generic.ConstructorParameters;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class ConstructorInitializedProperty : IDefinition
  {
    private readonly ICodeGenerator generator;

    public ConstructorInitializedProperty(string name, Type type)
    {
      generator = new PropertyInitializedGenerator(name, type);
    }

    public int SortOrder => 1;

    public bool CanGenerate(MemberInfo memberInfo) => memberInfo is null;

    public ICodeGenerator GetGenerator(GeneratorContext _) => generator;
  }
}
