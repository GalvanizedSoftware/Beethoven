using System;

namespace GalvanizedSoftware.Beethoven.Generic
{
  public class Parameter<T> : Parameter
  {
    private readonly Func<T> initializationFunc;

    public Parameter() :
      this(null, null)
    {
    }

    public Parameter(Func<T> initializationFunc) :
      this(null, initializationFunc)
    {
    }

    public Parameter(string name) :
      this(name, null)
    {
    }

    public Parameter(string name, Func<T> initializationFunc) :
      base(name, typeof(T))
    {
      this.initializationFunc = initializationFunc ?? (() => default);
    }

    public override object Create() =>
      initializationFunc();
  }

  public abstract class Parameter : IComparable<Parameter>
  {
    protected Parameter(string name, Type type)
    {
      Name = name;
      Type = type;
    }

    public string Name { get; }
    public Type Type { get; }
    public abstract object Create();

    public int CompareTo(Parameter other)
    {
      if (ReferenceEquals(this, other))
        return 0;
      if (ReferenceEquals(null, other))
        return 1;
      return string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(other.Name) ?
        string.Compare(Type.FullName, other.Type.FullName, StringComparison.Ordinal):
        string.Compare(Type.FullName + Name, other.Type.FullName + other.Name, StringComparison.Ordinal);
    }
  }
}
