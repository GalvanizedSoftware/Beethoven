namespace DefinitionLibrary
{
  class Level2Manager : IApprover
  {
    public bool Approve(double amount, ref string approvedBy)
    {
      if (amount > 1000)
        return false;
      approvedBy = "Level 2 manager";
      return true;
    }
  }
}
