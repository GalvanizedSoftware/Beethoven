using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility
{
  class LocalManager : IApprover
  {
    public bool Approve(double amount, ref string approvedBy)
    {
      if (amount > 200)
        return true;
      approvedBy = "Local manager";
      return false;
    }
  }
}
