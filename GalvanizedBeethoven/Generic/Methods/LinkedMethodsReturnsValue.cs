using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class LinkedMethodsReturnValue : Method, IObjectProvider
  {
    private readonly Method[] methodList;
    private readonly ObjectProviderHandler objectProviderHandler;

    public LinkedMethodsReturnValue(string name) :
      this(name, new Method[0])
    {
    }

    private LinkedMethodsReturnValue(LinkedMethodsReturnValue previous, Method newMethod) :
      this(previous.Name, previous.methodList.Append(newMethod).ToArray())
    {
    }

    private LinkedMethodsReturnValue(string name, Method[] methodList) :
      base(name, new MatchNothing()) // Matching is called through each method, not at this level
    {
      this.methodList = methodList;
      objectProviderHandler = new ObjectProviderHandler(methodList);
    }

    public LinkedMethodsReturnValue Add(Method method) =>
      new LinkedMethodsReturnValue(this, method);

    public LinkedMethodsReturnValue Func<TReturnType>(Func<TReturnType> func) =>
      Add(new FuncMethod<TReturnType>(Name, func));

    public LinkedMethodsReturnValue Lambda<T>(T actionOrFunc) =>
      Add(new LambdaMethod<T>(Name, actionOrFunc));

    public LinkedMethodsReturnValue MappedMethod(object instance, string targetName) =>
      Add(new MappedMethod(Name, instance, targetName));

    public LinkedMethodsReturnValue MappedMethod(object instance, MethodInfo methodInfo) =>
      Add(new MappedMethod(methodInfo, instance));

    public LinkedMethodsReturnValue AutoMappedMethod(object instance) =>
      Add(new MappedMethod(Name, instance, Name));

    public LinkedMethodsReturnValue InvertResult()
    {
      int index = methodList.Length - 1;
      methodList[index] = new InvertResultMethod(methodList[index]);
      return this;
    }

    public LinkedMethodsReturnValue SkipIf(Func<bool> condition) =>
      Add(ConditionCheckMethod.Create(Name, () => !condition()));

    public LinkedMethodsReturnValue SkipIf<T1>(Func<T1, bool> condition) =>
      Add(ConditionCheckMethod.Create<T1>(Name, arg1 => !condition(arg1)));

    public LinkedMethodsReturnValue SkipIf<T1, T2>(Func<T1, T2, bool> condition) =>
      Add(ConditionCheckMethod.Create<T1, T2>(Name, (arg1, arg2) => !condition(arg1, arg2)));

    public LinkedMethodsReturnValue SkipIfResultCondition<T>(Func<T, bool> condition) =>
      Add(new ReturnValueCheck<T>(Name, condition))
        .InvertResult();

    public LinkedMethodsReturnValue SkipIf(object instance, string targetName) =>
      Add(new PartialMatchMethod(Name, instance, targetName))
        .InvertResult();

    public LinkedMethodsReturnValue PartialMatchMethod(object instance, string targetName) =>
      Add(new PartialMatchMethod(Name, instance, targetName));

    public LinkedMethodsReturnValue PartialMatchMethod(object instance) =>
      Add(new PartialMatchMethod(Name, instance));

    public LinkedMethodsReturnValue PartialMatchMethod<TMain>(object instance, string mainParameterName) =>
      Add(new PartialMatchMethod(Name, instance, typeof(TMain), mainParameterName));

    public LinkedMethodsReturnValue PartialMatchLambda<T>(T actionOrFunc) =>
      Add(new PartialMatchLambda<T>(Name, actionOrFunc));

    public LinkedMethodsReturnValue PartialMatchAction<T>(ConstructorParameter parameter, Action<T> action) =>
      Add(new PartialMatchAction<T>(Name, parameter, action));

    public LinkedMethodsReturnValue PartialMatchAction<T>(Action<T> action) =>
      Add(new PartialMatchAction<T>(Name, action));

    public override void InvokeFindInstance(IInstanceMap instanceMap, Action<object> returnAction, 
      object[] parameters, Type[] genericArguments, MethodInfo methodInfo)
    {
      object returnValue = methodInfo.ReturnType.GetDefaultValue();
      (Type, string)[] parameterTypeAndNames = methodInfo.GetParameterTypeAndNames();
      foreach (Method method in methodList)
        if (!InvokeFirstMatch(instanceMap, method, ref returnValue, parameters, parameterTypeAndNames, genericArguments, methodInfo))
          break;
      returnAction(returnValue);
    }

    private bool InvokeFirstMatch(IInstanceMap instanceMap, Method method, ref object returnValue, object[] parameterValues,
      (Type, string)[] parameterTypeAndNames,
      Type[] genericArguments, MethodInfo methodInfo)
    {
      Type returnType = methodInfo.ReturnType;
      IMethodMatcher matcher = method.MethodMatcher;
      if (matcher.IsMatch(parameterTypeAndNames, genericArguments, returnType))
      {
        object returnValueLocal = returnValue;
        Action<object> returnAction = newValue => returnValueLocal = newValue;
        method.InvokeFindInstance(instanceMap, returnAction, parameterValues, genericArguments, methodInfo);
        returnValue = returnValueLocal;
        return true;
      }
      if (!matcher.IsMatchToFlowControlled(parameterTypeAndNames, genericArguments, returnType))
        throw new MissingMethodException();
      bool result = true;
      Action<object> localReturn = newValue => result = (bool)newValue;
      object[] newParameters = parameterValues.Append(returnValue).ToArray();
      method.InvokeFindInstance(instanceMap, localReturn, newParameters, genericArguments, methodInfo);
      for (int i = 0; i < parameterValues.Length; i++)
        parameterValues[i] = newParameters[i]; // In case of ref or out variables
      returnValue = newParameters.Last();
      return result;
    }

    public IEnumerable<TChild> Get<TChild>() => 
      objectProviderHandler.Get<TChild>();
  }
}