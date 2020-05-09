using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class LinkedMethods : MethodDefinition
  {
    private readonly MethodDefinition[] methodList;

    public LinkedMethods(string name) :
      this(name, Array.Empty<MethodDefinition>())
    {
    }

    private LinkedMethods(LinkedMethods previous, MethodDefinition newMethod) :
      this(previous.Name, previous.methodList.Append(newMethod).ToArray())
    {
    }

    private LinkedMethods(string name, MethodDefinition[] methodList) :
      base(name, new MatchAll(methodList.Select(method => method.MethodMatcher)))
    {
      this.methodList = methodList;
    }

    public LinkedMethods Add(MethodDefinition method) =>
      new LinkedMethods(this, method);

    public LinkedMethods Action(Action action) =>
      Add(new ActionMethod(Name, action));

    public LinkedMethods Action<T>(Action<T> action) =>
      Add(new ActionMethod(Name, action));

    public LinkedMethods MappedMethod(object instance, string targetName) =>
      Add(new MappedMethod(Name, instance, targetName));

    public LinkedMethods MappedMethod(object instance, MethodInfo methodInfo) =>
      Add(new MappedMethod(methodInfo, instance));

    public LinkedMethods AutoMappedMethod(object instance) =>
      Add(new MappedMethod(Name, instance));

    public LinkedMethods SkipIf(Func<bool> condition) =>
      Add(FlowControlMethod.Create(Name, () => !condition()));

    public LinkedMethods SkipIf<T1>(Func<T1, bool> condition) =>
      Add(FlowControlMethod.Create<T1>(Name, arg1 => !condition(arg1)));

    public LinkedMethods SkipIf<T1, T2>(Func<T1, T2, bool> condition) =>
      Add(FlowControlMethod.Create<T1, T2>(Name, (arg1, arg2) => !condition(arg1, arg2)));

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

    public override void Invoke(object localInstance, ref object returnAction, object[] parameterValues,
      Type[] genericArguments,
      MethodInfo methodInfo)
    {
      (Type, string)[] parameterTypeAndNames = methodInfo.GetParameterTypeAndNames();
      foreach (MethodDefinition method in methodList)
      {
        if (!InvokeFirstMatch(localInstance, method, parameterValues, parameterTypeAndNames, genericArguments, methodInfo))
          break;
      }
    }

    private bool InvokeFirstMatch(object localInstance,
      MethodDefinition method, object[] parameters, (Type, string)[] parameterTypes,
      Type[] genericArguments, MethodInfo methodInfo)
    {
      Type returnType = methodInfo?.ReturnType;
      if (method.MethodMatcher.IsMatchCheck(parameterTypes, genericArguments, returnType))
      {
        object returnValue = null;
        method.Invoke(localInstance, ref returnValue, parameters, genericArguments, methodInfo);
        return true;
      }
      if (!method.MethodMatcher.IsMatchCheck(parameterTypes, genericArguments, typeof(bool).MakeByRefType()))
        throw new MissingMethodException();
      object result = true;
      method.Invoke(localInstance, ref result, parameters, genericArguments, methodInfo);
      return (bool)result;
    }
  }
}