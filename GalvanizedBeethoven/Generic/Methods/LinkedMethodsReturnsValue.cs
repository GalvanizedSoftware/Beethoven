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
      this(name, Array.Empty<Method>())
    {
    }

    private LinkedMethodsReturnValue(LinkedMethodsReturnValue previous, Method newMethod) :
      this(previous.Name, previous.methodList.Append(newMethod).ToArray())
    {
    }

    private LinkedMethodsReturnValue(string name, Method[] methodList) :
      base(name, new MatchAll(methodList.Select(method => method.MethodMatcher)))
    {
      this.methodList = methodList;
      objectProviderHandler = new ObjectProviderHandler(methodList);
    }

    public LinkedMethodsReturnValue Add(Method method) =>
      new LinkedMethodsReturnValue(this, method);

    public LinkedMethodsReturnValue SimpleFunc<TReturnType>(Func<TReturnType> func) =>
      Add(new SimpleFuncMethod<TReturnType>(Name, func));

    public LinkedMethodsReturnValue FlowControl(Func<bool> func) =>
      Add(new FlowControlMethod(Name, func));

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
      Add(new InvertResultMethod(FlowControlMethod.Create(Name, condition)));

    public LinkedMethodsReturnValue SkipIf<T1>(Func<T1, bool> condition) =>
      Add(new InvertResultMethod(FlowControlMethod.Create<T1>(Name, condition)));

    public LinkedMethodsReturnValue SkipIf<T1, T2>(Func<T1, T2, bool> condition) =>
      Add(new InvertResultMethod(FlowControlMethod.Create<T1, T2>(Name, condition)));

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

    public LinkedMethodsReturnValue Action(Action action, IParameter localParameter = null) =>
      Add(new ActionMethod(Name, action, localParameter));

    public LinkedMethodsReturnValue Action<T>(Action<T> action, IParameter localParameter = null) =>
      Add(new ActionMethod(Name, action, localParameter));

    public LinkedMethodsReturnValue Func<TReturn>(Func<TReturn> func, IParameter localParameter = null) =>
      Add(new FuncMethod(Name, func, localParameter));

    public LinkedMethodsReturnValue Func<T, TReturn>(Func<T, TReturn> func, IParameter localParameter = null) =>
      Add(new FuncMethod(Name, func, localParameter));

    public LinkedMethodsReturnValue Func<T1, T2, TReturn>(Func<T1, T2, TReturn> func, IParameter localParameter = null) =>
      Add(new FuncMethod(Name, func, localParameter));

    public override void InvokeFindInstance(IInstanceMap instanceMap, ref object returnValue,
      object[] parameters, Type[] genericArguments, MethodInfo methodInfo)
    {
      if (parameters == null || methodInfo == null)
        throw new NullReferenceException();
      returnValue = methodInfo.GetDefaultReturnValue();
      (Type, string)[] parameterTypeAndNames = methodInfo.GetParameterTypeAndNames();
      foreach (Method method in methodList)
        if (!InvokeFirstMatch(instanceMap, method, ref returnValue, parameters, parameterTypeAndNames, genericArguments, methodInfo))
          break;
    }

    private bool InvokeFirstMatch(IInstanceMap instanceMap, Method method, ref object returnValue, object[] parameterValues,
      (Type, string)[] parameterTypeAndNames,
      Type[] genericArguments, MethodInfo methodInfo)
    {
      Type returnType = methodInfo.ReturnType;
      IMethodMatcher matcher = method.MethodMatcher;
      if (matcher.IsMatchCheck(parameterTypeAndNames, genericArguments, returnType))
      {
        method.InvokeFindInstance(instanceMap, ref returnValue, parameterValues, genericArguments, methodInfo);
        return true;
      }
      if (!matcher.IsMatchToFlowControlled(parameterTypeAndNames, genericArguments, returnType))
        throw new MissingMethodException();
      object[] newParameters = parameterValues.Append(returnValue).ToArray();
      object flowResult = true;
      method.InvokeFindInstance(instanceMap, ref flowResult, newParameters, genericArguments, methodInfo);
      for (int i = 0; i < parameterValues.Length; i++)
        parameterValues[i] = newParameters[i]; // In case of ref or out variables
      returnValue = newParameters.Last();
      return (bool)flowResult;
    }

    public IEnumerable<TChild> Get<TChild>() =>
      objectProviderHandler.Get<TChild>();
  }
}