using System;
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global

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

#pragma warning disable 67
    public event Action<string> MethodCalled = delegate { };
#pragma warning restore 67
  }
}
