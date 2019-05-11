using System;

namespace GalvanizedSoftware.Beethoven.Generic
{
  public class Parameter<T> : IParameter<T>
  {
    private readonly Func<T> initializationFunc;

    public Parameter(T instance)
    {
      initializationFunc = () => instance;
    }

    public Parameter(Func<T> initializationFunc)
    {
      this.initializationFunc = initializationFunc;
    }

    public T GetValue() =>
      initializationFunc();
  }
}
