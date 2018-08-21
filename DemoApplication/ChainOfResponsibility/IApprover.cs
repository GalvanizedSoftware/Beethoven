namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility
{
  public interface IApprover
  {
    bool Approve(double amount, ref string approvedBy);
  }
}
