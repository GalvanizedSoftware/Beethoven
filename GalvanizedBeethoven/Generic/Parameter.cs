using System;

namespace GalvanizedSoftware.Beethoven.Generic
{
  public class Parameter<T> : IParameter<T>
  {
    private readonly Func<T> initializationFunc;
    private readonly string name;

    public Parameter(Func<T> initializationFunc, string name)
    {
      this.initializationFunc = initializationFunc;
      this.name = name;
    }

    public T GetValue() =>
      initializationFunc();
  }
}
