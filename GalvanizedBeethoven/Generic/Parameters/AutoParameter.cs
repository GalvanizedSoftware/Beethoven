using System;

namespace GalvanizedSoftware.Beethoven.Generic.Parameters
{
  public class AutoParameter : IParameter
  {
    private readonly Delegate initializationFunc;

    public static AutoParameter Create<T>(Func<T> initializationFunc) =>
      new AutoParameter(typeof(T), initializationFunc);

    private AutoParameter(Type type, Delegate initializationFunc)
    {
      Type = type;
      this.initializationFunc = initializationFunc;
    }

    public object Create() =>
          initializationFunc.DynamicInvoke();

    public Type Type { get; }
    public int CompareTo(IParameter other) => 
      ReferenceEquals(this, other) ? 0 : 1;
  }
}
