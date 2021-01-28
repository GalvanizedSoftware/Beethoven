using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class ReturnValueCheckInverted<T> : MethodDefinition
  {
    private readonly Func<T, bool> condition;

    public ReturnValueCheckInverted(string name, Func<T, bool> condition) :
      base(name, new MatchFlowControl())
    {
      this.condition = value => !condition(value);
    }

    public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo)
    {
      yield return new ReturnValueCheckInvoker<T>(condition);
    }
  }
}