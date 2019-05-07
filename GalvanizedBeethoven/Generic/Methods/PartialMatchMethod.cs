using GalvanizedSoftware.Beethoven.Core.Binding;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class PartialMatchMethod : Method, IBindingParent
  {
    private readonly MethodInfo methodInfo;
    private readonly bool hasReturnType;
    private readonly (Type, string)[] localParameters;
    private object mainInstance;
    private readonly Type mainType;
    private readonly string mainParameterName;

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
      base(mainName, new MatchMethodNoReturn(instance.GetType(), targetName, mainParameterName))
    {
      Instance = instance;
      methodInfo = instance
        .GetType()
        .FindSingleMethod(targetName);
      localParameters = methodInfo.GetParameterTypeAndNames();
      hasReturnType = methodInfo.HasReturnType();
      this.mainParameterName = mainParameterName;
    }

    public object Instance { private get; set; }

    public void Bind(object target) => 
      mainInstance = target;

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo masterMethodInfo)
    {
      (Type, string)[] masterParameters = masterMethodInfo
        .GetParameterTypeAndNames()
        .AppendReturnValue(masterMethodInfo.ReturnType)
        .Append((mainType, mainParameterName))
        .ToArray();
      int[] indexes = localParameters
        .Select(item => Array.IndexOf(masterParameters, item))
        .ToArray();
      object[] inputParameters = GetInputParameters(parameters, masterParameters.Length);
      object[] localParameterValues = indexes
        .Select(index => inputParameters[index])
        .ToArray();
      object returnValue = methodInfo.Invoke(Instance, localParameterValues, genericArguments);
      // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
      localParameterValues.Zip(indexes,
        (value, index) => SetIfValid(parameters, index, value, masterParameters))
        .ToArray();
      if (hasReturnType)
        returnAction(returnValue);
    }

    private object[] GetInputParameters(object[] parameters, int length)
    {
      object[] returnValues = new object[length];
      for (int i = 0; i < parameters.Length; i++)
        returnValues[i] = parameters[i];
      returnValues[length - 1] = mainInstance;
      return returnValues;
    }

    private static object SetIfValid(object[] parameters, int index, object value, (Type, string)[] masterParameters)
    {
      if (index >= 0 && index < parameters.Length && masterParameters[index].Item1.IsByRef)
        parameters[index] = value;
      return null;
    }
  }
}