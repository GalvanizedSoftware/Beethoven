using GalvanizedSoftware.Beethoven.DemoApp.Extending.Services;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.DemoApp.Extending.Approvers;

namespace GalvanizedSoftware.Beethoven.DemoApp.Extending
{
  internal class Factory
  {
    private readonly MailService mailService;
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();
    private readonly Action<double, string, bool> approveAction;
    private readonly ApprovalAccount companyAccount;
    private readonly Action<ApprovalAccount> addAccountAction;

    public Factory(MailService mailService, Action<double, string, bool> approveAction,
      ApprovalAccount companyAccount, Action<ApprovalAccount> addAccountAction)
    {
      this.mailService = mailService;
      this.approveAction = approveAction;
      this.companyAccount = companyAccount;
      this.addAccountAction = addAccountAction;
    }

    public IApproverChain CreateChain(IEnumerable<IApprover> approvers)
    {
      return beethovenFactory.Generate<IApproverChain>(approvers
        .Select(WrapApprover)
        .Aggregate(new LinkedMethodsReturnValue(nameof(IApproverChain.Approve)),
          (value, approver) => value.AutoMappedMethod(approver)));
    }

    // A new composed class wrapping each approver
    private IApprover WrapApprover(IApprover approver)
    {
      ApprovalAccount approvalAccount = new ApprovalAccount(approver.GetType().Name);
      addAccountAction(approvalAccount);
      IApprover postApprover = beethovenFactory.Generate<IApprover>(
        new LinkedMethodsReturnValue(nameof(IApprover.Approve))
          .PartialMatchMethod(approvalAccount)
          .PartialMatchMethod(companyAccount)
      );
      Action<double, string, bool> currentResultAction = approveAction;
      currentResultAction += (amount, approvedBy, accepted) =>
      {
        if (accepted)
          postApprover.NotifyApprove(amount);
      };
      return beethovenFactory.Generate<IApprover>(
        new LinkedMethodsReturnValue(nameof(IApprover.Approve))
          .AutoMappedMethod(approver)
          .InvertResult()
          .SkipIfResultCondition<bool>(value => value)
          .PartialMatchMethod(new MailApprover(approver, mailService, currentResultAction))
      );
    }
  }
}
