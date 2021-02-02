using GalvanizedSoftware.Beethoven.Core.Methods;
using System;
using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class MappedMethodDelayed : MethodDefinition
  {
    private readonly Func<object, object> creatorFunc;
    private readonly MethodInfo methodInfo;

    public MappedMethodDelayed(MethodInfo methodInfo, Func<object, object> creatorFunc) :
      this(methodInfo?.Name, methodInfo, creatorFunc)
    {
    }

    private MappedMethodDelayed(string mainName, MethodInfo methodInfo, Func<object, object> creatorFunc) :
      base(mainName, new MatchMethodInfoExact(methodInfo))
    {
      this.methodInfo = methodInfo;
      this.creatorFunc = creatorFunc;
    }

    public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo)
    {
	    yield return new MappedInvokerDelayed(methodInfo, creatorFunc);
    }
  }
}