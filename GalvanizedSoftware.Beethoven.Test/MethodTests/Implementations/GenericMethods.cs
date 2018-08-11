using System;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests
{
  public class GenericMethods
  {
    public T Simple<T>()
    {
      MethodCalled(nameof(Simple));
      switch (Type.GetTypeCode(typeof(T)))
      {
        case TypeCode.Int32:
          return (T)(object)5;
        case TypeCode.String:
          return (T)(object)"abcd";
      }
      return default(T);
    }

    public void Parameter<T>(T value)
    {
      MethodCalled($"{nameof(Parameter)} {value}");
    }

    public void StructParameter<T>(T value) where T : struct
    {
      MethodCalled($"{nameof(StructParameter)} {value}");
    }

    public event Action<string> MethodCalled = delegate { };
  }
}
