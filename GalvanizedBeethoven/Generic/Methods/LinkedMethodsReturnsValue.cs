using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class LinkedMethodsReturnValue : Method
  {
    private readonly Method[] methodList;

    public LinkedMethodsReturnValue(string name) : base(name)
    {
      methodList = new Method[0];
    }

    private LinkedMethodsReturnValue(LinkedMethodsReturnValue previous, Method newMethod) :
      base(previous.Name)
    {
      methodList = previous.methodList.Append(newMethod).ToArray();
    }

    public LinkedMethodsReturnValue Func<TReturnType>(Func<TReturnType> func) =>
      new LinkedMethodsReturnValue(this, new FuncMethod<TReturnType>(Name, func));

    public LinkedMethodsReturnValue Lambda<T>(T actionOrFunc) =>
      new LinkedMethodsReturnValue(this, new LambdaMethod<T>(Name, actionOrFunc));

    public LinkedMethodsReturnValue MappedMethod(object instance, string targetName) =>
      new LinkedMethodsReturnValue(this, new MappedMethod(Name, instance, targetName));

    public LinkedMethodsReturnValue AutoMappedMethod(object instance) =>
      new LinkedMethodsReturnValue(this, new MappedMethod(Name, instance, Name));

    public LinkedMethodsReturnValue InvertResult()
    {
      int index = methodList.Length - 1;
      methodList[index] = new InvertResultMethod(methodList[index]);
      return this;
    }

    public LinkedMethodsReturnValue SkipIf(Func<bool> condition) =>
      new LinkedMethodsReturnValue(this, ConditionCheckMethod.Create(Name, () => !condition()));

    public LinkedMethodsReturnValue SkipIf<T1>(Func<T1, bool> condition) =>
      new LinkedMethodsReturnValue(this, ConditionCheckMethod.Create<T1>(Name, arg1 => !condition(arg1)));

    public LinkedMethodsReturnValue SkipIf<T1, T2>(Func<T1, T2, bool> condition) =>
      new LinkedMethodsReturnValue(this, ConditionCheckMethod.Create<T1, T2>(Name, (arg1, arg2) => !condition(arg1, arg2)));

    public LinkedMethodsReturnValue PartialMatchMethod(object instance, string targetName) =>
      new LinkedMethodsReturnValue(this, new PartialMatchMethod(Name, instance, targetName));

    public LinkedMethodsReturnValue PartialMatchMethod(object instance) =>
      new LinkedMethodsReturnValue(this, new PartialMatchMethod(Name, instance));

    public override bool IsMatch(IEnumerable<(Type, string)> parameters, Type[] genericArguments, Type returnType)
    {
      List<(Type, string)> parameterList = parameters.ToList();
      return methodList.Any(method => method.IsMatch(parameterList, genericArguments, returnType)) ||
             methodList.Any(method => method.IsMatch(parameterList.Append((returnType, "returnValue")), genericArguments, typeof(bool)));
    }

    internal override void Invoke(Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo methodInfo)
    {
      object returnValue = methodInfo.ReturnType.GetDefaultValue();
      (Type, string)[] parameterTypeAndNames = methodInfo.GetParameterTypeAndNames();
      foreach (Method method in methodList)
      {
        if (!InvokeFirstMatch(method, ref returnValue, parameters, parameterTypeAndNames, genericArguments, methodInfo))
          break;
      }
      returnAction(returnValue);
    }

    private bool InvokeFirstMatch(Method method, ref object returnValue, object[] parameterValues, (Type, string)[] parameterTypeAndNames,
      Type[] genericArguments, MethodInfo methodInfo)
    {
      Type returnType = methodInfo.ReturnType;
      if (method.IsMatch(parameterTypeAndNames, genericArguments, returnType))
      {
        object returnValueLocal = returnValue;
        Action<object> returnAction = newValue => returnValueLocal = newValue;
        method.Invoke(returnAction, parameterValues, genericArguments, methodInfo);
        returnValue = returnValueLocal;
        return true;
      }
      if (!method.IsMatch(parameterTypeAndNames.Append((returnType.MakeByRefType(), "returnType")), genericArguments, typeof(bool)))
        throw new MissingMethodException();
      bool result = true;
      Action<object> localReturn = newValue => result = (bool)newValue;
      object[] newParameters = parameterValues.Append(returnValue).ToArray();
      method.Invoke(localReturn, newParameters, genericArguments, methodInfo);
      for (int i = 0; i < parameterValues.Length; i++)
        parameterValues[i] = newParameters[i]; // In case of ref or out variables
      returnValue = newParameters.Last();
      return result;
    }
  }
}