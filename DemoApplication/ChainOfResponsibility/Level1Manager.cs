namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility
{
  internal class Level1Manager : IApprover
  {
    public bool Approve(double amount, ref string approvedBy)
    {
      if (amount > 10000)
        return true;
      approvedBy = "Level 1 manager";
      return false;
    }
  }
}
