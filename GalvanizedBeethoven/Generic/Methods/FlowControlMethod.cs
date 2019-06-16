using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class FlowControlMethod : Method
  {
    private readonly Delegate func;

    internal FlowControlMethod(string name, Delegate func) :
      base(name, new MatchFlowControl())
    {
      this.func = func;
    }

    public override void Invoke(object _, ref object returnValue,
      object[] parameters, Type[] __, MethodInfo ___) =>
      returnValue = func.DynamicInvoke(parameters.SkipLast().ToArray());
  }
}