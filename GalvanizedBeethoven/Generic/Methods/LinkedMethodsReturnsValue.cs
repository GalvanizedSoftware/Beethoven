using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class LinkedMethodsReturnValue : MethodDefinition
  {
    private readonly MethodDefinition[] methodList;
    private readonly MethodInfo methodInfo;

    public LinkedMethodsReturnValue(MethodInfo methodInfo) :
      this(methodInfo ?? throw new NullReferenceException(), Array.Empty<MethodDefinition>())
    {
    }

    private LinkedMethodsReturnValue(LinkedMethodsReturnValue previous, MethodDefinition newMethod) :
      this(previous.methodInfo, previous.methodList.Append(newMethod).ToArray())
    {
    }

    private LinkedMethodsReturnValue(MethodInfo methodInfo, MethodDefinition[] methodList) :
      base(methodInfo.Name, new MatchMethodInfoExact(methodInfo))
    {
      this.methodList = methodList;
      this.methodInfo = methodInfo;
    }

    public static LinkedMethodsReturnValue Create<T>(string name, int index = 0) =>
      new LinkedMethodsReturnValue(typeof(T)
        .GetAllMethods(name)
        .Where((_, i) => i == index)
        .FirstOrDefault());


    public LinkedMethodsReturnValue Add(MethodDefinition method) =>
      new LinkedMethodsReturnValue(this, method);

    public LinkedMethodsReturnValue SimpleFunc<TReturnType>(Func<TReturnType> func) =>
      Add(new SimpleFuncMethod<TReturnType>(Name, func));

    public LinkedMethodsReturnValue FlowControl(Func<bool> func) =>
      Add(new FlowControlMethod(Name, func));

    public LinkedMethodsReturnValue MappedMethod(object instance, string targetName) =>
      Add(new MappedMethod(Name, instance, targetName));

    public LinkedMethodsReturnValue MappedMethod(object instance, MethodInfo mappedMethodInfo) =>
      Add(new MappedMethod(mappedMethodInfo, instance));

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
      Add(new InvertResultMethod(FlowControlMethod.Create(Name, condition)));

    public LinkedMethodsReturnValue SkipIf<T1, T2>(Func<T1, T2, bool> condition) =>
      Add(new InvertResultMethod(FlowControlMethod.Create(Name, condition)));

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

    public LinkedMethodsReturnValue Action(Action action) =>
      Add(new ActionMethod(Name, action));

    public LinkedMethodsReturnValue Action<T>(Action<T> action) =>
      Add(new ActionMethod(Name, action));

    public LinkedMethodsReturnValue Func<TReturn>(Func<TReturn> func) =>
      Add(new FuncMethod(Name, func));

    public LinkedMethodsReturnValue Func<T, TReturn>(Func<T, TReturn> func) =>
      Add(new FuncMethod(Name, func));

    public LinkedMethodsReturnValue Func<T1, T2, TReturn>(Func<T1, T2, TReturn> func) =>
      Add(new FuncMethod(Name, func));

    public override void Invoke(object localInstance, ref object returnValue,
      object[] parameters, Type[] genericArguments, MethodInfo invokeMethodInfo)
    {
      if (parameters == null || invokeMethodInfo == null)
        throw new NullReferenceException();
      returnValue = invokeMethodInfo.GetDefaultReturnValue();
      (Type, string)[] parameterTypeAndNames = invokeMethodInfo.GetParameterTypeAndNames();
      foreach (MethodDefinition method in methodList)
        if (!InvokeFirstMatch(localInstance, method, ref returnValue, parameters, parameterTypeAndNames, genericArguments, invokeMethodInfo))
          break;
    }

    private static bool InvokeFirstMatch(object localInstance, MethodDefinition method, ref object returnValue, object[] parameterValues,
      (Type, string)[] parameterTypeAndNames,
      Type[] genericArguments, MethodInfo methodInfo)
    {
      Type returnType = methodInfo.ReturnType;
      IMethodMatcher matcher = method.MethodMatcher;
      if (matcher.IsMatchCheck(parameterTypeAndNames, genericArguments, returnType))
      {
        method.Invoke(localInstance, ref returnValue, parameterValues, genericArguments, methodInfo);
        return true;
      }
      if (!matcher.IsMatchToFlowControlled(parameterTypeAndNames, genericArguments, returnType))
        throw new MissingMethodException();
      object[] newParameters = parameterValues.Append(returnValue).ToArray();
      object flowResult = true;
      method.Invoke(localInstance, ref flowResult, newParameters, genericArguments, methodInfo);
      for (int i = 0; i < parameterValues.Length; i++)
        parameterValues[i] = newParameters[i]; // In case of ref or out variables
      returnValue = newParameters.Last();
      return (bool)flowResult;
    }
  }
}