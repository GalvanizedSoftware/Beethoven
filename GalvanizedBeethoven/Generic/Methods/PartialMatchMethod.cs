using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class PartialMatchMethod : MethodDefinition
  {
    private readonly MethodInfo methodInfo;
    private readonly Type mainType;
    private readonly string mainParameterName;
    private readonly object instance;

    public PartialMatchMethod(string mainName, object instance) :
      this(mainName, instance, mainName, "")
    {
    }

    public PartialMatchMethod(string mainName, object instance, string targetName) :
      this(mainName, instance, targetName, "")
    {
    }

    public PartialMatchMethod(string mainName, object instance, Type mainType, string mainParameterName) :
      this(mainName, instance, mainName, mainParameterName)
    {
      this.mainType = mainType;
    }

    public PartialMatchMethod(string mainName, object instance, string targetName, string mainParameterName) :
      this(mainName, targetName,
        instance?.GetType().FindSingleMethod(targetName),
        null, mainParameterName, instance)
    {
    }

    private PartialMatchMethod(string mainName, string targetName, MethodInfo methodInfo, Type mainType,
      string mainParameterName, object instance) :
      base(mainName, GetMethodMatcher(instance, targetName, mainParameterName))
    {
      this.methodInfo = methodInfo;
      this.instance = instance;
      this.mainType = mainType;
      this.mainParameterName = mainParameterName;
    }

    public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo)
    {
	    yield return new PartialMatchInvoker(methodInfo, mainType, mainParameterName, instance);
    }

    private static IMethodMatcher GetMethodMatcher(object instance, string targetName, string mainParameterName)
    {
      Type type = instance?.GetType();
      return new MatchMethodNoReturn(type, targetName, mainParameterName);
    }
  }
}