using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  internal class FallbackMethodDefinition : MethodDefinition
  {
    private readonly MethodDefinition master;

    public FallbackMethodDefinition(MethodDefinition master) :
      base(master.Name, master.MethodMatcher)
    {
      this.master = master;
    }

    public override int SortOrder => 2;

    public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo) => 
      master.GetInvokers(memberInfo);
  }
}