namespace GalvanizedSoftware.Beethoven.DemoApp.Extending.Approvers
{
  public interface IApprover
  {
    bool Approve(double amount, ref string approvedBy);
  }
}
