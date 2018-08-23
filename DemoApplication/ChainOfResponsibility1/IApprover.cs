namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility1
{
  public interface IApprover
  {
    bool Approve(double amount, ref string approvedBy);
  }
}
