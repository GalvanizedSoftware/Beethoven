using GalvanizedSoftware.Beethoven.DemoApp.Common;
using GalvanizedSoftware.Beethoven.DemoApp.Extending.Approvers;
using GalvanizedSoftware.Beethoven.DemoApp.Extending.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

namespace GalvanizedSoftware.Beethoven.DemoApp.Extending
{
  internal class ExtendingViewModel
  {
    private readonly Factory factory;
    private readonly MailService mailService;
    private readonly IApproverChain approverChain;
    private readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

    public ExtendingViewModel()
    {
      Dictionary<IApprover, string> approvers = new Dictionary<IApprover, string>()
      {
        {new Myself(), "me"},
        {new LocalManager(), "pat@theonlycompany.com"},
        {new Level2Manager(), "lucy@theonlycompany.com"},
        {new Level1Manager(), "john@theonlycompany.com"}
      };
      mailService = new MailService(approvers);
      ApprovalAccounts.Add(CompanyAccount);
      factory = new Factory(mailService, OnApprovedRejected, CompanyAccount, account => ApprovalAccounts.Add(account));
      approverChain = factory.CreateChain(approvers.Keys);
      AddRequest1Command = new Command(() => RequestApproval(1));
      AddRequest100Command = new Command(() => RequestApproval(100));
      AddRequest1000Command = new Command(() => RequestApproval(1000));
      AddRequestALotCommand = new Command(() => RequestApproval(1000000));
    }

    private void RequestApproval(double amount)
    {
      string pendingApprover = approverChain.Approve(amount);
      OrderItems.Add(string.IsNullOrEmpty(pendingApprover) ?
        $"Expense of {amount} € was not approved by anyone" :
        $"Expense of {amount} € is pending at  {pendingApprover}");
    }

    private void OnApprovedRejected(double amount, string approver, bool accepted)
    {
      dispatcher.BeginInvoke(new Action(() =>
          OrderItems.Add(string.IsNullOrEmpty(approver) ? $"Expense of {amount} € was not approved by anyone" :
            accepted ? $"Expense of {amount} € was approved by {approver}" :
            $"Expense of {amount} € was rejected by {approver}"))
        );
    }

    public ObservableCollection<string> OrderItems { get; } = new ObservableCollection<string>();

    public ObservableCollection<ApprovalAccount> ApprovalAccounts { get; } = new ObservableCollection<ApprovalAccount>();

    public ApprovalAccount CompanyAccount { get; } = new ApprovalAccount("Company");
    
    public ICommand AddRequest1Command { get; }
    public ICommand AddRequest100Command { get; }
    public ICommand AddRequest1000Command { get; }
    public ICommand AddRequestALotCommand { get; }
  }
}