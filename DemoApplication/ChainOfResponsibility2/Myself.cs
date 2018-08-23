namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility2
{
  internal class Myself : IApprover
  {
    public bool Approve(double amount, ref string approvedBy)
    {
      if (amount > 10)
        return false;
      approvedBy = "Myself";
      TotalApproval += amount;
      return true;
    }

    public double TotalApproval { get; private set; }
  }
}
