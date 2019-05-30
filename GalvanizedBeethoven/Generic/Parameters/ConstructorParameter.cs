using System;

namespace GalvanizedSoftware.Beethoven.Generic.Parameters
{
  public class ConstructorParameter : IParameter
  {
    private readonly string typeFullName;

    public static ConstructorParameter Create<T>(string name) =>
      new ConstructorParameter(name, typeof(T));

    public static ConstructorParameter Create<T>() =>
      new ConstructorParameter(null, typeof(T));

    public ConstructorParameter(string name, Type type)
    {
      Name = name;
      Type = type;
      typeFullName = Type.FullName;
    }

    public string Name { get; }
    public Type Type { get; }

    public bool Equals(IParameter other) => 
      ReferenceEquals(this, other) || 
      other is ConstructorParameter constructorParameter && 
      Equals(constructorParameter.Name, constructorParameter.Type.FullName);

    public bool Equals((Type, string) other) =>
      Equals(other.Item2, other.Item1.FullName);

    private bool Equals(string otherName, string otherFullName) =>
      string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(otherName) ?
        string.Equals(typeFullName, otherFullName, StringComparison.Ordinal) :
        string.Equals(typeFullName + Name, otherFullName + otherName, StringComparison.Ordinal);
  }
}
