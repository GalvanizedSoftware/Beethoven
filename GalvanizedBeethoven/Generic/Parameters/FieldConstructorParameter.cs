using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Parameters
{
  public class FieldConstructorParameter : ICodeGenerator
  {
    public static FieldConstructorParameter Create<T>() =>
      new FieldConstructorParameter(typeof(T));

    public FieldConstructorParameter(Type type)
    {
      Type = type ?? throw new NullReferenceException();
      Name = $"field{type.GetFormattedName()}{new TagGenerator()}";
    }

    public string Name { get; }
    public Type Type { get; }

    public IEnumerable<string> Generate(GeneratorContext generatorContext) =>
      generatorContext?.MemberInfo switch
      {
        FieldInfo _ => GenerateField(),
        ConstructorInfo _ => GenerateConstructor(),
        _ => Enumerable.Empty<string>()
      };
    private IEnumerable<string> GenerateField()
    {
      yield return $"{Type.GetFullName()} {Name};";
    }

    private IEnumerable<string> GenerateConstructor()
    {
      yield return $"{Type.GetFullName()} {Name}";
      yield return $"this.{Name} = {Name};";
    }

    public ICodeGenerator GetGenerator() =>
      this;
  }
}
