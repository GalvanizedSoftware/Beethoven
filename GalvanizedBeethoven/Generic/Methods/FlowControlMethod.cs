using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class FlowControlMethod : Method
  {
    private readonly Delegate func;
    private readonly (Type, string)[] localParameters;

    public static FlowControlMethod Create(string name, Func<bool> func) =>
      new FlowControlMethod(name, func);

    public static FlowControlMethod Create<T1>(string name, Func<T1, bool> func) =>
      new FlowControlMethod(name, func);

    public static FlowControlMethod Create<T1, T2>(string name, Func<T1, T2, bool> func) =>
      new FlowControlMethod(name, func);

    internal FlowControlMethod(string name, Delegate func) :
      base(name, new MatchFlowControl())
    {
      this.func = func;
      localParameters = func.Method.GetParameterTypeAndNames();
    }

    public override void Invoke(object _, ref object returnValue,
      object[] parameters, Type[] __, MethodInfo masterMethodInfo) =>
      returnValue = func.DynamicInvoke(
        masterMethodInfo.GetLocalParameters(parameters, localParameters));
  }
}