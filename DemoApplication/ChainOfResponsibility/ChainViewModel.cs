using GalvanizedSoftware.Beethoven.DemoApp.Common;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility
{
  internal class ChainViewModel
  {
    private readonly Factory factory = new Factory();
    private readonly IApproverChain approverChain;

    public ChainViewModel()
    {
      approverChain = factory.CreateChain(
        new Myself(),
        new LocalManager(),
        new Level2Manager(),
        new Level1Manager());
      AddRequest1Command = new Command(() => RequestApproval(1));
      AddRequest100Command = new Command(() => RequestApproval(100));
      AddRequest1000Command = new Command(() => RequestApproval(1000));
      AddRequestALotCommand = new Command(() => RequestApproval(1000000));
    }

    private void RequestApproval(double amount)
    {
      string approver = approverChain.Approve(amount);

      OrderItems.Add(string.IsNullOrEmpty(approver) ?
        $"Expence of {amount} € was not approved" :
        $"Expence of {amount} € was approved by {approver}");
    }

    public ObservableCollection<string> OrderItems { get; } = new ObservableCollection<string>();

    public ICommand AddRequest1Command { get; }
    public ICommand AddRequest100Command { get; }
    public ICommand AddRequest1000Command { get; }
    public ICommand AddRequestALotCommand { get; }
  }
}