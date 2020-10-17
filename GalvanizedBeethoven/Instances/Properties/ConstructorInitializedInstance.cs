using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Generic.ConstructorParameters;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Implementations.Properties
{
  public class ConstructorInitializedInstance : IDefinition
  {
    private readonly ICodeGenerator generator;

    public ConstructorInitializedInstance(string name, Type type)
    {
      generator = new PropertyInitializedGenerator(name, type);
    }

    public int SortOrder => 1;

    public bool CanGenerate(MemberInfo memberInfo) => memberInfo is null;

    public ICodeGenerator GetGenerator(GeneratorContext _) => generator;
  }
}
