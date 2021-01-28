using GalvanizedSoftware.Beethoven.Core.Methods;
using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class ParameterMappedMethod : MethodDefinition
  {
    private readonly MethodInfo methodInfo;

    public ParameterMappedMethod(MethodInfo methodInfo) :
      this(methodInfo?.Name, methodInfo)
    {
    }

    private ParameterMappedMethod(string mainName, MethodInfo methodInfo) :
      base(mainName, new MatchMethodInfoExact(methodInfo))
    {
      this.methodInfo = methodInfo;
      methodInfo.HasReturnType();
    }

    public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo)
    {
	    yield return new ParameterMappedInvoker(methodInfo);
    }
  }
}