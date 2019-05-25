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
}
