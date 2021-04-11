namespace DefinitionLibrary
{
  public interface IApprover
  {
    bool Approve(double amount, ref string approvedBy);
  }
}
