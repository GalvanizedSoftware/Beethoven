using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Interfaces
{
  public interface IInvoker
  {
    bool Invoke(object localInstance,
      ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo);
  }
}