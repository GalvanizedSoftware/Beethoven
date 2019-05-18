using System;

namespace GalvanizedSoftware.Beethoven.Generic
{
  public class Parameter<T> : Parameter
  {
    private readonly Func<T> initializationFunc;

    public Parameter(T instance, object definition) :
      base(definition)
    {
      initializationFunc = () => instance;
    }

    public Parameter(Func<T> initializationFunc, object definition) :
      base(definition)
    {
      this.initializationFunc = initializationFunc;
    }

    public override object Create() =>
      initializationFunc();
  }

  public abstract class Parameter
  {
    protected Parameter(object definition)
    {
      Definition = definition;
    }

    public object Definition { get; }

    public abstract object Create();
  }
}
