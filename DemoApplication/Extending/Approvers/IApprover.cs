namespace GalvanizedSoftware.Beethoven.DemoApp.Extending
{
  public interface IApprover
  {
    bool Approve(double amount, ref string approvedBy);
  }
}
