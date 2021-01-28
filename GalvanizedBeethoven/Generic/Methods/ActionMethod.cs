using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class ActionMethod : MethodDefinition
  {
    private readonly Delegate action;

    public static ActionMethod Create(string mainName, Action action) =>
      new(mainName, GetValues(action));

    public static ActionMethod Create<T>(string mainName, Action<T> action) =>
      new(mainName, GetValues(action));

    public static ActionMethod Create<T1, T2>(string mainName, Action<T1, T2> action) =>
      new(mainName, GetValues(action));

    public static ActionMethod Create<T1, T2, T3>(string mainName, Action<T1, T2, T3> action) =>
      new(mainName, GetValues(action));

    public ActionMethod(string mainName, Delegate action) :
      this(mainName, GetValues(action))
    {
      this.action = action;
    }

    private ActionMethod(string mainName, (IMethodMatcher MethodMatcher, Delegate Action) values) :
      base(mainName, values.MethodMatcher)
    {
      action = values.Action;
    }

    private static (MatchActionPartially, Delegate) GetValues(Delegate action)
    {
      (Type, string)[] localParameters = action?.Method.GetParameterTypeAndNames() ??
        throw new NullReferenceException();
      return (new(localParameters), action);
    }

    public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo)
    {
      yield return ActionInvoker.Create(action);
    }
  }
}