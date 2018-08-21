using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility
{
  class Level2Manager : IApprover
  {
    public bool Approve(double amount, ref string approvedBy)
    {
      if (amount > 1000)
        return true;
      approvedBy = "Level 2 manager";
      return false;
    }
  }
}
