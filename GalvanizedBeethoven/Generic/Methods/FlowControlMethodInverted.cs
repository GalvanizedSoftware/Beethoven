using System;
using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class FlowControlMethodInverted : MethodDefinition
  {
    private readonly Delegate func;

    public static FlowControlMethodInverted Create(string name, Func<bool> func) =>
      new(name, func);

    public static FlowControlMethodInverted Create<T1>(string name, Func<T1, bool> func) =>
      new(name, func);

    public static FlowControlMethodInverted Create<T1, T2>(string name, Func<T1, T2, bool> func) =>
      new(name, func);

    private FlowControlMethodInverted(string name, Delegate func) :
      base(name, new MatchFlowControl())
    {
      this.func = func ?? throw new NullReferenceException();
    }

    public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo)
    {
      yield return new FlowControlInvertedInvoker(func);
    }
  }
}