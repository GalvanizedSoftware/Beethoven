using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class SimpleFuncMethod<TReturnType> : MethodDefinition
  {
    private readonly Func<TReturnType> func;

    public SimpleFuncMethod(string name, Func<TReturnType> func) :
      base(name, new MatchNoParametersAndReturnType<TReturnType>())
    {
      this.func = func;
    }

    public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo)
    {
	    yield return new FuncInvoker(func);
    }
  }
}