using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class ActionMethod : Method
  {
    private class ConstructorValues
    {
      public (Type, string)[] LocalParameters { get; }
      public IMethodMatcher MethodMatcher { get; }
      public Delegate Action { get; }
      public int? ParameterIndex { get; }
      public IParameter Parameter { get; }

      public ConstructorValues(
        (Type, string)[] localParameters,
        IMethodMatcher methodMatcher,
        Delegate action,
        int? parameterIndex,
        IParameter parameter)
      {
        LocalParameters = localParameters;
        MethodMatcher = methodMatcher;
        Action = action;
        ParameterIndex = parameterIndex;
        Parameter = parameter;
      }
    }

    private readonly Delegate action;
    private readonly (Type, string)[] localParameters;
    private readonly int? parameterIndex;

    public static ActionMethod Create(string mainName, Action action, IParameter parameter = null) =>
      new ActionMethod(mainName, GetValues(action, parameter));

    public static ActionMethod Create<T>(string mainName, Action<T> action, IParameter parameter = null) =>
      new ActionMethod(mainName, GetValues(action, parameter));

    public static ActionMethod Create<T1, T2>(string mainName, Action<T1, T2> action, IParameter parameter = null) =>
      new ActionMethod(mainName, GetValues(action, parameter));

    public static ActionMethod Create<T1, T2, T3>(string mainName, Action<T1, T2, T3> action, IParameter parameter = null) =>
      new ActionMethod(mainName, GetValues(action, parameter));

    public ActionMethod(string mainName, Delegate action, IParameter parameter = null) :
      this(mainName, GetValues(action, parameter))
    {
    }

    private ActionMethod(string mainName, ConstructorValues values) :
      base(mainName, values.MethodMatcher, values.Parameter)
    {
      action = values.Action;
      localParameters = values.LocalParameters;
      parameterIndex = values.ParameterIndex;
    }

    private static ConstructorValues GetValues(Delegate action, IParameter parameter)
    {
      (Type, string)[] localParameters = action.Method.GetParameterTypeAndNames();
      int? parameterIndex = localParameters
        .Select((tuple, i) => (tuple, (int?)i))
        .FirstOrDefault(outerTuple => parameter?.Equals(outerTuple.tuple) == true)
        .Item2;
      IMethodMatcher methodMatcher = new MatchActionPartially(
        localParameters.ExceptIndex(parameterIndex.GetValueOrDefault(-1)));
      return new ConstructorValues(localParameters, methodMatcher, action, parameterIndex, parameter);
    }

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo masterMethodInfo)
    {
      (Type, string)[] masterParameters = masterMethodInfo
        .GetParameterTypeAndNames()
        .AppendReturnValue(masterMethodInfo.ReturnType)
        .ToArray();
      object[] localParameterValues = localParameters
        .Select(item => Array.IndexOf(masterParameters, item))
        .Select(index => index < 0 ? null : parameters[index])
        .ToArray();
      if (parameterIndex != null)
        localParameterValues[parameterIndex.Value] = localInstance;
      action.DynamicInvoke(localParameterValues);
    }
  }
}