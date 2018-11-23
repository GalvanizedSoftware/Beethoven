namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility2
{
  internal class Factory
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();

    public IApprover CreateLink(IApprover currentApprover, IApprover nextApprover)
    {
      return beethovenFactory.Generate<IApprover>(
        new ApproverLink(currentApprover, nextApprover)
        );
    }
  }
}
