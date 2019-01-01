using GalvanizedSoftware.Beethoven.DemoApp.Common;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility2
{
  internal class ChainViewModel
  {
    private readonly Factory factory = new Factory();
    private readonly IApprover approverChain;
    private readonly Myself myself = new Myself();
    private readonly LocalManager localManager = new LocalManager();
    private readonly Level2Manager level2Manager = new Level2Manager();
    private readonly Level1Manager level1Manager = new Level1Manager();

    public ChainViewModel()
    {

      approverChain =
        factory.CreateLink(myself,
        factory.CreateLink(localManager,
        factory.CreateLink(level2Manager,
        level1Manager)));
      AddRequest1Command = new Command(() => RequestApproval(1));
      AddRequest100Command = new Command(() => RequestApproval(100));
      AddRequest1000Command = new Command(() => RequestApproval(1000));
      AddRequestALotCommand = new Command(() => RequestApproval(1000000));
      PrintStatesCommand = new Command(PrintStates);
    }

    private void PrintStates()
    {
      OrderItems.Add($"I approved: {myself.TotalApproval}");
      OrderItems.Add($"Local Manager approved: {localManager.TotalApproval}");
      OrderItems.Add($"Level 2 Manager approved: {level2Manager.TotalApproval}");
      OrderItems.Add($"Level 1 Manager approved: {level1Manager.TotalApproval}");
    }

    private void RequestApproval(double amount)
    {
      string approver = null;
      OrderItems.Add(approverChain.Approve(amount, ref approver) ?
        $"Expense of {amount} € was approved by {approver}" :
        $"Expense of {amount} € was not approved");
    }

    public ObservableCollection<string> OrderItems { get; } = new ObservableCollection<string>();

    public ICommand AddRequest1Command { get; }
    public ICommand AddRequest100Command { get; }
    public ICommand AddRequest1000Command { get; }
    public ICommand AddRequestALotCommand { get; }
    public ICommand PrintStatesCommand { get; }
  }
}