using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class PartialMatchLambda<T> : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly object target;
    private readonly bool hasReturnType;
    private readonly (Type, string)[] localParameters;

    static PartialMatchLambda()
    {
      typeof(T).CheckForDelegateType();
    }

    public PartialMatchLambda(string mainName, T actionOrFunc) :
      this(mainName, actionOrFunc as Delegate)
    {
    }

    public PartialMatchLambda(string mainName, Delegate lambdaDelegate) :
      this(mainName, lambdaDelegate.Target, lambdaDelegate.Method)
    {
    }

    private PartialMatchLambda(string mainName, object target, MethodInfo lambdaMethodInfo) :
      base(mainName, new MatchLambdaPartiallyNoReturn(lambdaMethodInfo))
    {
      methodInfo = lambdaMethodInfo;
      localParameters = lambdaMethodInfo.GetParameterTypeAndNames();
      hasReturnType = lambdaMethodInfo.HasReturnType();
      this.target = target;
    }

    public override void Invoke(object localInstance, Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo masterMethodInfo)
    {
      (Type, string)[] masterParameters = masterMethodInfo
        .GetParameterTypeAndNames()
        .AppendReturnValue(masterMethodInfo.ReturnType)
        .ToArray();
      int[] indexes = localParameters
        .Select(item => Array.IndexOf(masterParameters, item))
        .ToArray();
      object[] localParameterValues = indexes
        .Select(index => parameters[index])
        .ToArray();
      object returnValue = methodInfo.Invoke(target, localParameterValues, genericArguments);
      // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
      localParameterValues.Zip(indexes,
        (value, index) => SetIfValid(parameters, index, value, masterParameters))
        .ToArray();
      if (hasReturnType)
        returnAction(returnValue);
    }

    private static object SetIfValid(object[] parameters, int index, object value, (Type, string)[] masterParameters)
    {
      if (index >= 0 && index < parameters.Length && masterParameters[index].Item1.IsByRef)
        parameters[index] = value;
      return null;
    }
  }
}