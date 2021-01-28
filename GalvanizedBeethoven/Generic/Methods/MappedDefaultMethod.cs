using System;
using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class MappedDefaultMethod : MethodDefinition
  {
    private readonly MethodInfo methodInfo;
    private readonly Func<MethodInfo, object[], object> mainFunc;

    public override int SortOrder => 2;

    public MappedDefaultMethod(MethodInfo methodInfo, Func<MethodInfo, object[], object> mainFunc) :
      base(methodInfo?.Name, new MatchMethodInfoExact(methodInfo))
    {
      this.methodInfo = methodInfo ?? throw new NullReferenceException();
      this.mainFunc = mainFunc ?? throw new NullReferenceException();
    }

    public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo)
    {
	    yield return new DefaultMethodInfoInvoker(methodInfo, mainFunc);
    }
  }
}