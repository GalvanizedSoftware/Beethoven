using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Interfaces;

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
      new(typeof(T)
        .GetAllMethods(name)
        .Where((_, i) => i == index)
        .FirstOrDefault());


    public LinkedMethodsReturnValue Add(MethodDefinition method) =>
      new(this, method);

    public LinkedMethodsReturnValue SimpleFunc<TReturnType>(Func<TReturnType> func) =>
      Add(new SimpleFuncMethod<TReturnType>(Name, func));

    public LinkedMethodsReturnValue FlowControl(Func<bool> func) =>
      Add(FlowControlMethod.Create(Name, func));

    public LinkedMethodsReturnValue MappedMethod(object instance, string targetName) =>
      Add(new MappedMethod(Name, instance, targetName));

    public LinkedMethodsReturnValue MappedMethod(object instance, MethodInfo mappedMethodInfo) =>
      Add(new MappedMethod(mappedMethodInfo, instance));

    public LinkedMethodsReturnValue AutoMappedMethod(object instance) =>
      Add(new MappedMethod(Name, instance, Name));

    public LinkedMethodsReturnValue InvertResult() => 
	    Add(new InvertResultMethod(Name));

    public LinkedMethodsReturnValue SkipIf(Func<bool> condition) =>
      Add(FlowControlMethodInverted.Create(Name, condition));

    public LinkedMethodsReturnValue SkipIf<T1>(Func<T1, bool> condition) =>
      Add(FlowControlMethodInverted.Create(Name, condition));

    public LinkedMethodsReturnValue SkipIf<T1, T2>(Func<T1, T2, bool> condition) =>
      Add(FlowControlMethodInverted.Create(Name, condition));

    public LinkedMethodsReturnValue SkipIfResultCondition<T>(Func<T, bool> condition) =>
      Add(new ReturnValueCheckInverted<T>(Name, condition));

    public LinkedMethodsReturnValue SkipIf(object instance, string targetName) =>
      Add(new PartialMatchMethod(Name, instance, targetName).ToInvertedFlowControl());

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
      Add(FuncMethod.Create(Name, func));

    public LinkedMethodsReturnValue Func<T, TReturn>(Func<T, TReturn> func) =>
      Add(FuncMethod.Create(Name, func));

    public LinkedMethodsReturnValue Func<T1, T2, TReturn>(Func<T1, T2, TReturn> func) =>
      Add(FuncMethod.Create(Name, func));

    public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo) => 
      methodList.SelectMany(definition => definition.GetInvokers(memberInfo));
  }
}