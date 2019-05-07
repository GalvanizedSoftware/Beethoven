﻿using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class LinkedMethods : Method, IObjectProvider
  {
    private readonly Method[] methodList;
    private readonly ObjectProviderHandler objectProviderHandler;

    public LinkedMethods(string name) :
      this(name, new Method[0])
    {
    }

    private LinkedMethods(LinkedMethods previous, Method newMethod) :
      this(previous.Name, previous.methodList.Append(newMethod).ToArray())
    {
    }

    private LinkedMethods(string name, Method[] methodList) : 
      base(name, new MatchLinked(methodList.Select(method => method.MethodMatcher)))
    {
      this.methodList = methodList;
      objectProviderHandler = new ObjectProviderHandler(methodList);
    }

    public LinkedMethods Add(Method method) => 
      new LinkedMethods(this, method);

    public LinkedMethods Lambda<T>(T actionOrFunc) =>
      Add(new LambdaMethod<T>(Name, actionOrFunc));

    public LinkedMethods MappedMethod(object instance, string targetName) =>
      Add(new MappedMethod(Name, instance, targetName));

    public LinkedMethods MappedMethod(object instance, MethodInfo methodInfo) =>
      Add(new MappedMethod(instance, methodInfo));

    public LinkedMethods AutoMappedMethod(object instance) =>
      Add(new MappedMethod(Name, instance));

    public LinkedMethods SkipIf(Func<bool> condition) =>
      Add(ConditionCheckMethod.Create(Name, () => !condition()));

    public LinkedMethods SkipIf<T1>(Func<T1, bool> condition) =>
      Add(ConditionCheckMethod.Create<T1>(Name, arg1 => !condition(arg1)));

    public LinkedMethods SkipIf<T1, T2>(Func<T1, T2, bool> condition) =>
      Add(ConditionCheckMethod.Create<T1, T2>(Name, (arg1, arg2) => !condition(arg1, arg2)));

    public LinkedMethods SkipIf(object instance, string targetName) =>
      Add(new PartialMatchMethod(Name, instance, targetName))
        .InvertResult();

    public LinkedMethods PartialMatchMethod(object instance, string targetName) =>
      Add(new PartialMatchMethod(Name, instance, targetName));

    public LinkedMethods PartialMatchMethod(object instance) =>
      Add(new PartialMatchMethod(Name, instance));

    public LinkedMethods PartialMatchMethod<TMain>(object instance, string mainParameterName) =>
      Add(new PartialMatchMethod(Name, instance, typeof(TMain), mainParameterName));

    public LinkedMethods InvertResult()
    {
      int index = methodList.Length - 1;
      methodList[index] = new InvertResultMethod(methodList[index]);
      return this;
    }

    internal override void Invoke(Action<object> returnAction, object[] parameterValues, Type[] genericArguments, MethodInfo methodInfo)
    {
      (Type, string)[] parameterTypeAndNames = methodInfo.GetParameterTypeAndNames();
      foreach (Method method in methodList)
      {
        if (!InvokeFirstMatch(method, parameterValues, parameterTypeAndNames, genericArguments, methodInfo))
          break;
      }
    }

    private bool InvokeFirstMatch(Method method, object[] parameters, (Type, string)[] parameterTypes,
      Type[] genericArguments, MethodInfo methodInfo)
    {
      Type returnType = methodInfo.ReturnType;
      if (method.MethodMatcher.IsMatch(parameterTypes, genericArguments, returnType))
      {
        method.Invoke(null, parameters, genericArguments, methodInfo);
        return true;
      }
      if (!method.MethodMatcher.IsMatch(parameterTypes, genericArguments, typeof(bool).MakeByRefType()))
        throw new MissingMethodException();
      bool result = true;
      Action<object> localReturn = newValue => result = (bool)newValue;
      method.Invoke(localReturn, parameters, genericArguments, methodInfo);
      return result;
    }

    public IEnumerable<TChild> Get<TChild>() => 
      objectProviderHandler.Get<TChild>();
  }
}