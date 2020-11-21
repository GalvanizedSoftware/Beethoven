using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class PartialMatchMethod : MethodDefinition
  {
    private readonly MethodInfo methodInfo;
    private readonly bool hasReturnType;
    private readonly (Type, string)[] localParameters;
    private readonly Type mainType;
    private readonly string mainParameterName;
    private readonly object instance;

    public PartialMatchMethod(string mainName, object instance) :
      this(mainName, instance, mainName, "")
    {
    }

    public PartialMatchMethod(string mainName, object instance, string targetName) :
      this(mainName, instance, targetName, "")
    {
    }

    public PartialMatchMethod(string mainName, object instance, Type mainType, string mainParameterName) :
      this(mainName, instance, mainName, mainParameterName)
    {
      this.mainType = mainType;
    }

    public PartialMatchMethod(string mainName, object instance, string targetName, string mainParameterName) :
      this(mainName, targetName,
        instance?.GetType().FindSingleMethod(targetName),
        null, mainParameterName, instance)
    {
    }

    private PartialMatchMethod(string mainName, string targetName, MethodInfo methodInfo, Type mainType,
      string mainParameterName, object instance) :
      base(mainName, GetMethodMatcher(instance, targetName, mainParameterName))
    {
      this.methodInfo = methodInfo;
      this.instance = instance;
      localParameters = methodInfo.GetParameterTypeAndNames();
      hasReturnType = methodInfo.HasReturnType();
      this.mainType = mainType;
      this.mainParameterName = mainParameterName;
    }

    public override void Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo masterMethodInfo)
    {
      (Type, string)[] masterParameters = masterMethodInfo
        .GetParameterTypeAndNames()
        .AppendReturnValue(masterMethodInfo?.ReturnType)
        .Append((mainType, mainParameterName))
        .ToArray();
      int[] indexes = localParameters
        .Select(item => Array.IndexOf(masterParameters, item))
        .ToArray();
      object[] inputParameters = GetInputParameters(localInstance, parameters, masterParameters.Length);
      object[] localParameterValues = indexes
        .Select(index => inputParameters[index])
        .ToArray();
      object invokeResult = methodInfo.Invoke(instance, localParameterValues, genericArguments);
      if (hasReturnType)
        returnValue = invokeResult;
      // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
      _ = localParameterValues.Zip(indexes,
        (value, index) => SetIfValid(parameters, index, value, masterParameters))
        .ToArray();
    }

    private object[] GetInputParameters(object localInstance, object[] parameters, int length)
    {
      if (parameters == null || length == 0)
        return Array.Empty<object>();
      object[] returnValues = new object[length];
      for (int i = 0; i < parameters.Length; i++)
        returnValues[i] = parameters[i];
      returnValues[length - 1] = localInstance;
      return returnValues;
    }

    private static object SetIfValid(object[] parameters, int index, object value, (Type, string)[] masterParameters)
    {
      if (index >= 0 && index < parameters.Length && masterParameters[index].Item1.IsByRefence())
        parameters[index] = value;
      return null;
    }

    private static IMethodMatcher GetMethodMatcher(object instance, string targetName, string mainParameterName)
    {
      Type type = instance?.GetType();
      return new MatchMethodNoReturn(type, targetName, mainParameterName);
    }
  }
}