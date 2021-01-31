using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
	public abstract class MethodDefinition : DefaultDefinition, IDefinitions
	{
		protected MethodDefinition(string name, IMethodMatcher methodMatcher)
		{
			Name = name;
			MethodMatcher = methodMatcher ?? new MatchNothing();
		}

		public string Name { get; }
		public IMethodMatcher MethodMatcher { get; }

		public MethodDefinition ToFlowControl() =>
			new FlowControlMethodDefinition(this, false);

		public MethodDefinition ToInvertedFlowControl() =>
			new FlowControlMethodDefinition(this, true);

		public override bool CanGenerate(MemberInfo memberInfo) =>
			MethodMatcher.IsMatchIgnoreGeneric(memberInfo as MethodInfo, Name);

		public override ICodeGenerator GetGenerator(GeneratorContext _) => null;

		public IEnumerable<IDefinition> GetDefinitions<T>() where T : class
		{
			yield return this;
		}
	}
}
