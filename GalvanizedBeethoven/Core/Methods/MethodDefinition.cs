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
  public abstract class MethodDefinition : DefaultDefinition, IMainTypeUser
  {
    private string invokerName;
    private Func<object> invokerFactory;

    protected MethodDefinition(string name, IMethodMatcher methodMatcher)
    {
      Name = name;
      MethodMatcher = methodMatcher ?? new MatchNothing();
    }

    public string Name { get; }
    public IMethodMatcher MethodMatcher { get; }

    public void Set(Type setMainType)
    {
      MemberInfoList memberInfoList = MemberInfoListCache.Get(setMainType);
      var methodInfos = memberInfoList
        .MethodInfos
        .Where(CanGenerate)
        .ToArray();
      switch (methodInfos.Length)
      {
        case 0:
          return;
      }
      if (methodInfos.Length != 1)
      {

      }
      MethodInfo methodInfo = methodInfos.FirstOrDefault();
      if (methodInfo != null)
      {
        invokerName = memberInfoList.GetMethodInvokerName(methodInfo);
        invokerFactory = () => new RealMethodInvoker(methodInfo, this);
      }
    }

    public virtual void Invoke(object localInstance,
      ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo) =>
      throw new MissingMemberException(methodInfo?.DeclaringType?.FullName, methodInfo?.Name);

    public override bool CanGenerate(MemberInfo memberInfo) =>
      MethodMatcher.IsMatchIgnoreGeneric(memberInfo as MethodInfo, Name);

    public override ICodeGenerator GetGenerator(GeneratorContext _) => null;


    public override IEnumerable<(string, object)> GetFields()
    {
      if (invokerName != null)
        yield return (invokerName, invokerFactory());
    }

  }
}
