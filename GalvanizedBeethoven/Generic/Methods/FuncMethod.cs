using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class FuncMethod : MethodDefinition
  {
    private readonly Delegate func;

    public static FuncMethod Create<TReturn>(string mainName, Func<TReturn> func) =>
      new(mainName, func);

    public static FuncMethod Create<T1, TReturn>(string mainName, Func<T1, TReturn> func) =>
      new(mainName, func);

    public static FuncMethod Create<T1, T2, TReturn>(string mainName, Func<T1, T2, TReturn> func) =>
      new(mainName, func);

    private FuncMethod(string mainName, Delegate func) :
      this(mainName, func?.Method)
    {
      this.func = func;
    }

    private FuncMethod(string mainName, MethodInfo methodInfo) :
      base(mainName, new MatchFuncPartially(methodInfo))
    {
    }

    public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo)
    {
      yield return new FuncInvoker(func);
    }
  }
}