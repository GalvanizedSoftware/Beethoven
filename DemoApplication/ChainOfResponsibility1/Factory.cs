using System.Linq;
using GalvanizedSoftware.Beethoven.Generic.Methods;

namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility1
{
  internal class Factory
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();

    public IApproverChain CreateChain(params IApprover[] approvers)
    {
      LinkedMethodsReturnValue linkedMethods = approvers
        .Aggregate(new LinkedMethodsReturnValue(nameof(IApproverChain.Approve)),
        (value, approver) => value.AutoMappedMethod(approver));
      return beethovenFactory.Generate<IApproverChain>(linkedMethods);
    }
  }
}
