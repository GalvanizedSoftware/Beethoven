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
        .Aggregate(LinkedMethodsReturnValue.Create<IApproverChain>(nameof(IApproverChain.Approve)),
        (value, approver) => value
          .AutoMappedMethod(approver)
          .InvertResult());
      /*
       * In the linked methods, the method return true to continue execution, and also to interrupt.
       * It is illogical that 'Approve' should return false for non-approved.
       * InvertResult simply inverts the result of a bool-method.
       */
      return beethovenFactory.Generate<IApproverChain>(linkedMethods);
    }
  }
}
