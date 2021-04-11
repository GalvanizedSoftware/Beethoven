using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
  public class FlowControlInvertedInvoker : IInvoker
  {
    private readonly Delegate func;
    private readonly (Type, string)[] localParameters;

    public FlowControlInvertedInvoker(Delegate func)
    {
      this.func = func ?? throw new NullReferenceException();
      localParameters = func.Method.GetParameterTypeAndNames();
    }

    public bool Invoke(object _, ref object returnValue,
      object[] parameters, Type[] __, MethodInfo masterMethodInfo) =>
      !(bool)func.DynamicInvoke(
        masterMethodInfo.GetLocalParameters(parameters, localParameters));
  }
}