using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Parameters
{
  public class PropertyConstructorInitialized : ICodeGenerator, IParameter, IDefinition
  {
    private readonly string typeFullName;

    public static PropertyConstructorInitialized Create<T>(string name) =>
      new PropertyConstructorInitialized(name, typeof(T));

    public static PropertyConstructorInitialized Create<T>() =>
      new PropertyConstructorInitialized(null, typeof(T));

    public PropertyConstructorInitialized(string name, Type type)
    {
      Name = name;
      Type = type ?? throw new NullReferenceException();
      typeFullName = Type.FullName;
    }

    public string Name { get; }
    public Type Type { get; }

    public int SortOrder => 1;

    public bool Equals(IParameter other) =>
      ReferenceEquals(this, other) ||
      other is PropertyConstructorInitialized constructorParameter &&
      Equals(constructorParameter.Name, constructorParameter.Type.FullName);

    public bool Equals((Type, string) other) =>
      Equals(other.Item2, other.Item1.FullName);

    private bool Equals(string otherName, string otherFullName) =>
      string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(otherName) ?
        string.Equals(typeFullName, otherFullName, StringComparison.Ordinal) :
        string.Equals(typeFullName + Name, otherFullName + otherName, StringComparison.Ordinal);

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      string parameterName = $"{char.ToUpper(Name[0], CultureInfo.InvariantCulture)}{Name.Substring(1)}";
      yield return $"{Type.GetFullName()} {parameterName}";
      yield return $"this.{Name} = {parameterName};";
    }

    public bool CanGenerate(MemberInfo memberInfo) =>
      memberInfo switch
      {
        ConstructorInfo _ => true,
        _ => false,
      };

    public ICodeGenerator GetGenerator() => this;
  }
}
