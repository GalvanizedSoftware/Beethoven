using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Interfaces;

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
      new(this, method);

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
      Add(FlowControlMethodInverted.Create(Name, condition));

    public LinkedMethods SkipIf<T1>(Func<T1, bool> condition) =>
      Add(FlowControlMethodInverted.Create(Name, condition));

    public LinkedMethods SkipIf<T1, T2>(Func<T1, T2, bool> condition) =>
      Add(FlowControlMethodInverted.Create(Name, condition));

    public LinkedMethods SkipIf(object instance, string targetName) =>
      Add(new PartialMatchMethod(Name, instance, targetName).ToInvertedFlowControl());

    public LinkedMethods PartialMatchMethod(object instance, string targetName) =>
      Add(new PartialMatchMethod(Name, instance, targetName));

    public LinkedMethods PartialMatchMethod(object instance) =>
      Add(new PartialMatchMethod(Name, instance));

    public LinkedMethods PartialMatchMethod<TMain>(object instance, string mainParameterName) =>
      Add(new PartialMatchMethod(Name, instance, typeof(TMain), mainParameterName));

    public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo) => 
      methodList.SelectMany(definition => definition.GetInvokers(memberInfo));
  }
}