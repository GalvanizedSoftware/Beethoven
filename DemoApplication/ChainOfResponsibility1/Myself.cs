namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility1
{
  internal class Myself : IApprover
  {
    public bool Approve(double amount, ref string approvedBy)
    {
      if (amount > 10)
        return false;
      approvedBy = "Myself";
      return true;
    }
  }
}
