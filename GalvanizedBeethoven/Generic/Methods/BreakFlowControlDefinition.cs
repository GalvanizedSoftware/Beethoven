using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
	public class BreakFlowControlDefinition : MethodDefinition
	{
		private readonly MethodDefinition master;

		public BreakFlowControlDefinition(MethodDefinition master) :
			base(master.Name, master.MethodMatcher)
		{
			this.master = master;
		}

		public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo) =>
			master.GetInvokers(memberInfo)
				.Append(new BreakFlowControlInvoker());

		public override bool CanGenerate(MemberInfo memberInfo) => 
			master.CanGenerate(memberInfo);

		public override ICodeGenerator GetGenerator(MemberInfo memberInfo) =>
			master.GetGenerator(memberInfo);

		public override int SortOrder => master.SortOrder;
	}
}