using GalvanizedSoftware.Beethoven.Core.Binding;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class PartialMatchLamda<T> : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly bool hasReturnType;
    private readonly (Type, string)[] localParameters;
    private readonly string mainParameterName = "";
    private readonly Delegate lambdaDelegate;

    public PartialMatchLamda(string mainName, T actionOrFunc) :
      base(mainName)
    {
      lambdaDelegate = actionOrFunc as Delegate;
      if (lambdaDelegate == null)
        throw new InvalidCastException("You must supply a func or delegate");
      methodInfo = lambdaDelegate.Method;
      localParameters = methodInfo.GetParameterTypeAndNames();
      hasReturnType = methodInfo.ReturnType != typeof(void);
    }

    public override bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType)
    {
      if (methodInfo.ReturnType == typeof(bool) && returnType.IsByRef == false)
        return false;
      (Type, string)[] checkedParameters = localParameters
        .Where(tuple => tuple.Item2 != mainParameterName)
        .ToArray();
      return checkedParameters
          .All(parameters.Contains);
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo masterMethodInfo)
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
      object returnValue = this.methodInfo.Invoke(lambdaDelegate.Target, localParameterValues, genericArguments);
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