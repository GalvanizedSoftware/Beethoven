namespace DefinitionLibrary
{
  class LocalManager : IApprover
  {
    public bool Approve(double amount, ref string approvedBy)
    {
      if (amount > 200)
        return false;
      approvedBy = "Local manager";
      return true;
    }
  }
}
