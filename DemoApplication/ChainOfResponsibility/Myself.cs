namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility
{
  internal class Myself : IApprover
  {
    public bool Approve(double amount, ref string approvedBy)
    {
      if (amount > 10)
        return true;
      approvedBy = "Myself";
      return false;
    }
  }
}
