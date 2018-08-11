using System;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests.Implementations
{
  public class GenericMethods2
  {
    internal int SimpleInt()
    {
      return 5;
    }

    internal string SimpleString()
    {
      return "abcd";
    }

    public T Simple<T>()
    {
      return default(T);
    }

    public void Parameter<T>(T value)
    {
    }

    public void StructParameter<T>(T value) where T : struct
    {
    }

    public event Action<string> MethodCalled = delegate { };
  }
}
