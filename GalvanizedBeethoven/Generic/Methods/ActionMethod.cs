using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class ActionMethod : MethodDefinition
  {
    private class ConstructorValues
    {
      public (Type, string)[] LocalParameters { get; }
      public IMethodMatcher MethodMatcher { get; }
      public Delegate Action { get; }

      public ConstructorValues(
        (Type, string)[] localParameters,
        IMethodMatcher methodMatcher,
        Delegate action)
      {
        LocalParameters = localParameters;
        MethodMatcher = methodMatcher;
        Action = action;
      }
    }

    private readonly Delegate action;
    private readonly (Type, string)[] localParameters;

    public static ActionMethod Create(string mainName, Action action) =>
      new ActionMethod(mainName, GetValues(action));

    public static ActionMethod Create<T>(string mainName, Action<T> action) =>
      new ActionMethod(mainName, GetValues(action));

    public static ActionMethod Create<T1, T2>(string mainName, Action<T1, T2> action) =>
      new ActionMethod(mainName, GetValues(action));

    public static ActionMethod Create<T1, T2, T3>(string mainName, Action<T1, T2, T3> action) =>
      new ActionMethod(mainName, GetValues(action));

    public ActionMethod(string mainName, Delegate action) :
      this(mainName, GetValues(action))
    {
    }

    private ActionMethod(string mainName, ConstructorValues values) :
      base(mainName, values.MethodMatcher)
    {
      action = values.Action;
      localParameters = values.LocalParameters;
    }

    private static ConstructorValues GetValues(Delegate action)
    {
      (Type, string)[] localParameters = action?.Method.GetParameterTypeAndNames() ??
        throw new NullReferenceException();
      return new ConstructorValues(localParameters, new MatchActionPartially(localParameters), action);
    }

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo masterMethodInfo) => 
      action.DynamicInvoke(masterMethodInfo.GetLocalParameters(parameters, localParameters));
  }
}