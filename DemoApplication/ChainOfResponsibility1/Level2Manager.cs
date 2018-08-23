namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility1
{
  class Level2Manager : IApprover
  {
    public bool Approve(double amount, ref string approvedBy)
    {
      if (amount > 1000)
        return true;
      approvedBy = "Level 2 manager";
      return false;
    }
  }
}
