namespace GalvanizedSoftware.Beethoven.DemoApp.Extending.Approvers
{
  // ReSharper disable once InconsistentNaming
  public static class IApproverExtension
  {
    public static void NotifyApprove(this IApprover approver, double amount)
    {
      string _ = "";
      approver.Approve(amount, ref _);
    }
  }
}
