namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility
{
  public interface IApproverChain
  {
    string Approve(double amount);
  }
}