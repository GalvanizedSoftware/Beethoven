namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility2
{
  class LocalManager : IApprover
  {
    public bool Approve(double amount, ref string approvedBy)
    {
      if (amount > 200)
        return false;
      approvedBy = "Local manager";
      TotalApproval += amount;
      return true;
    }

    public double TotalApproval { get; private set; }
  }
}
