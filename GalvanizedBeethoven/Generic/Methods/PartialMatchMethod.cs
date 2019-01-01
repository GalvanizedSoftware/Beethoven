using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class PartialMatchMethod : Method
  {
    private readonly MethodInfo methodInfo;
    private readonly bool hasReturnType;
    private readonly (Type, string)[] localParameters;

    public PartialMatchMethod(MethodInfo methodInfo) :
      base(methodInfo.Name)
    {
      this.methodInfo = methodInfo;
      localParameters = methodInfo.GetParameterTypeAndNames();
      hasReturnType = methodInfo.ReturnType != typeof(void);
    }

    public PartialMatchMethod(string mainName, object instance) :
      this(mainName, instance, mainName)
    {
    }

    public PartialMatchMethod(string mainName, object instance, string targetName) :
      base(mainName)
    {
      Instance = instance;
      methodInfo = instance
        .GetType()
        .FindSingleMethod(targetName);
      localParameters = methodInfo.GetParameterTypeAndNames();
      hasReturnType = methodInfo.ReturnType != typeof(void);
    }

    public object Instance { private get; set; }

    public override bool IsMatch((Type, string)[] parameters, Type[] genericArguments, Type returnType)
    {
      if (methodInfo.ReturnType == typeof(bool) && returnType != typeof(bool) && parameters.LastOrDefault().Item1?.IsByRef == false)
        return false;
      // Check for generic
      return localParameters
          .All(parameters.Contains);
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo masterMethodInfo)
    {
      (Type, string)[] masterParameters = masterMethodInfo.GetParameterTypeAndNames().AppendReturnValue(masterMethodInfo.ReturnType);
      int[] indexes = localParameters
        .Select(item => Array.IndexOf(masterParameters, item))
        .ToArray();
      object[] localParameterValues = indexes
        .Select(index => parameters[index])
        .ToArray();
      object returnValue = methodInfo.Invoke(Instance, localParameterValues, genericArguments);
      // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
      localParameterValues.Zip(indexes, (value, index) => parameters[index] = value).ToArray();
      if (hasReturnType)
        returnAction(returnValue);
    }
  }
}