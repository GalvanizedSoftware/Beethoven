namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility1
{
  public interface IApproverChain
  {
    string Approve(double amount);
  }
}