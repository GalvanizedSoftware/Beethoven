using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
	class InvertMasterResultMethod : MethodDefinition
	{
		public InvertMasterResultMethod(string name) :
			base(name, new MatchFlowControl())
		{
		}

		public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo)
		{
			yield return new InvertResultInvoker();
		}
	}
}
