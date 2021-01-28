using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public class InvertResultInvoker : IInvoker
  {
    public bool Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo)
    {
      if (returnValue is bool value)
        returnValue = !value;
      return true;
    }
  }
}
