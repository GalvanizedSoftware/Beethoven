namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility2
{
  public interface IApprover
  {
    bool Approve(double amount, ref string approvedBy);
    double TotalApproval { get; }
  }
}
