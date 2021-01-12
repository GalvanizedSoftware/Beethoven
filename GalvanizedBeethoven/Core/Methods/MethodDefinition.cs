using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.Definitions;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public abstract class MethodDefinition : DefaultDefinition, IDefinitions, IMainTypeUser, IFieldMaps
  {
    protected string invokerName;
    protected Func<object> invokerFactory;
    protected MethodInfo linkedMethodInfo;

    protected MethodDefinition(string name, IMethodMatcher methodMatcher)
    {
      Name = name;
      MethodMatcher = methodMatcher ?? new MatchNothing();
    }

    public string Name { get; }
    public IMethodMatcher MethodMatcher { get; }

    public virtual void Set(Type setMainType)
    {
      MemberInfoList memberInfoList = MemberInfoListCache.Get(setMainType);
      MethodInfo[] methodInfos = memberInfoList
        .MethodInfos
        .Where(CanGenerate)
        .ToArray();
      if (methodInfos.Length == 0)
        return;
      if (methodInfos.Length != 1)
      {

      }
      linkedMethodInfo = methodInfos.Single();
      if (linkedMethodInfo?.IsGenericMethodDefinition == true)
        return;
      invokerName = memberInfoList.GetMethodInvokerName(linkedMethodInfo);
      invokerFactory = () => new RealMethodInvoker(linkedMethodInfo, this);
    }

    public virtual void Invoke(object localInstance,
      ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo) =>
      throw new MissingMemberException(methodInfo?.DeclaringType?.FullName, methodInfo?.Name);

    public override bool CanGenerate(MemberInfo memberInfo) =>
      MethodMatcher.IsMatchIgnoreGeneric(memberInfo as MethodInfo, Name);

    public override ICodeGenerator GetGenerator(GeneratorContext _) => null;


    public virtual IEnumerable<(string, object)> GetFields()
    {
      if (invokerName != null)
        yield return (invokerName, invokerFactory());
      else
      {

      }
    }

    public IEnumerable<IDefinition> GetDefinitions<T>() where T : class
    {
      Set(typeof(T));
      if (linkedMethodInfo?.IsGenericMethodDefinition == true)
      {
        yield return new GenericMethodWrapper(linkedMethodInfo, this);
      }
      else
      {
        yield return this;
      }
    }
  }
}
