using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public class FlowControlMethodDefinition : MethodDefinition
  {
    private readonly MethodDefinition methodDefinition;
    private readonly bool isInverted;

    public FlowControlMethodDefinition(MethodDefinition methodDefinition, bool isInverted) :
      base(methodDefinition.Name, methodDefinition.MethodMatcher)
    {
      this.methodDefinition = methodDefinition;
      this.isInverted = isInverted;
    }

    public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo)
    {
      IEnumerable<IInvoker> invokers = methodDefinition.GetInvokers(memberInfo);
      yield return isInverted ?
        new FlowControlInvertedInvokers(invokers) :
        new FlowControlInvokers(invokers);
    }
  }
}