using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
	public class InvertResultMethod : MethodDefinition
	{
		private readonly MethodDefinition master;

		public InvertResultMethod(string name, MethodDefinition master) :
			base(master.Name, master.MethodMatcher)
		{
			this.master = master;
		}

		public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo) =>
			master
				.GetInvokers(memberInfo)
				.Select(invoker => new FlowControlMasterInvertedInvoker(invoker));
	}
}
