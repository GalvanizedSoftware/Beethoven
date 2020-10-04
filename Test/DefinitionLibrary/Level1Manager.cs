namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility1
{
  internal class Level1Manager : IApprover
  {
    public bool Approve(double amount, ref string approvedBy)
    {
      if (amount > 10000)
        return false;
      approvedBy = "Level 1 manager";
      return true;
    }
  }
}
