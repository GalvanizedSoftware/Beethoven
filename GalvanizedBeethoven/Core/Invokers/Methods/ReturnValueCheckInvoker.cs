using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public class ReturnValueCheckInvoker<T> : IInvoker
  {
    private readonly Func<T, bool> condition;

    public ReturnValueCheckInvoker(Func<T, bool> condition)
    {
      this.condition = condition;
    }

    public bool Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo _) =>
      condition((T)parameters.LastOrDefault());
  }
}