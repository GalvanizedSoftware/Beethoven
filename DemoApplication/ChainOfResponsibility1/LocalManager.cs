namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility1
{
  class LocalManager : IApprover
  {
    public bool Approve(double amount, ref string approvedBy)
    {
      if (amount > 200)
        return true;
      approvedBy = "Local manager";
      return false;
    }
  }
}
