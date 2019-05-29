using System;

namespace GalvanizedSoftware.Beethoven.Generic.Parameters
{
  public class ConstructorParameter : IParameter
  {
    public static ConstructorParameter Create<T>(string name) =>
      new ConstructorParameter(name, typeof(T));

    public ConstructorParameter(string name, Type type)
    {
      Name = name;
      Type = type;
    }

    public string Name { get; }
    public Type Type { get; }

    public int CompareTo(IParameter other)
    {
      if (ReferenceEquals(null, other))
        return 1;
      if (ReferenceEquals(this, other))
        return 0;
      if (!(other is ConstructorParameter constructorParameter))
        return 1;
      return string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(constructorParameter.Name) ?
        string.Compare(Type.FullName, other.Type.FullName, StringComparison.Ordinal) :
        string.Compare(Type.FullName + Name, other.Type.FullName + constructorParameter.Name, StringComparison.Ordinal);
    }
  }
}
