using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GalvanizedSoftware.Beethoven.DemoApp.Extending.Services
{
  internal class MailService
  {
    private readonly Dictionary<IApprover, string> mails;
    private readonly Random random = new Random();

    public MailService(IDictionary<IApprover, string> mails)
    {
      this.mails = new Dictionary<IApprover, string>(mails);
    }

    public void Send(IApprover approver, double amount, string message, Action<double, string, bool> acceptanceAction)
    {
      Task.Run(() =>
      {
        Thread.Sleep(5000);
        acceptanceAction(amount, mails[approver], random.NextDouble() > 0.2);
      });
    }
  }
}