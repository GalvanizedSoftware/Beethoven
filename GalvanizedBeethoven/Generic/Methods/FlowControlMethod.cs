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