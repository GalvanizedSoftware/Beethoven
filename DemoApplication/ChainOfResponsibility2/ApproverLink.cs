namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility2
{
  internal class ApproverLink
  {
    private readonly IApprover current;
    private readonly IApprover next;

    public ApproverLink(IApprover current, IApprover next)
    {
      this.current = current;
      this.next = next;
    }

    public bool Approve(double amount, ref string approvedBy)
    {
      if (current.Approve(amount, ref approvedBy))
        return true;
      return next.Approve(amount, ref approvedBy);
    }
  }
}
