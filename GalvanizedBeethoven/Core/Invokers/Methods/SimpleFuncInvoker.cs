using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public class SimpleFuncInvoker<TReturnType> : IInvoker
  {
    private readonly Func<TReturnType> func;

    public SimpleFuncInvoker(string name, Func<TReturnType> func)
    {
      this.func = func;
    }

    public bool Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo _)
    {
      returnValue = func();
      return true;
    }
  }
}