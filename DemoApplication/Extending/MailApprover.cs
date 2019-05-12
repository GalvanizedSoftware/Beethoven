using GalvanizedSoftware.Beethoven.DemoApp.Extending.Services;
using System;

namespace GalvanizedSoftware.Beethoven.DemoApp.Extending
{
  internal class MailApprover
  {
    private readonly IApprover approver;
    private readonly MailService mailService;
    private readonly Action<double, string, bool> acceptanceAction;

    public MailApprover(IApprover approver, MailService mailService, Action<double, string, bool> acceptanceAction)
    {
      this.approver = approver;
      this.mailService = mailService;
      this.acceptanceAction = acceptanceAction;
    }

    public void Approve(double amount)
    {
      mailService.Send(approver, amount, $"Do you approve an expense of {amount} $ ?", acceptanceAction);
    }
  }
}
