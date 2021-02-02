using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Methods;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
	public abstract class MethodDefinition : DefaultDefinition
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

		public MethodDefinition BreakFlowControl() =>
			new BreakFlowControlDefinition(this);

		public override bool CanGenerate(MemberInfo memberInfo) =>
			MethodMatcher.IsMatchIgnoreGeneric(memberInfo as MethodInfo, Name);

		public override ICodeGenerator GetGenerator(GeneratorContext _) => null;

		protected bool IsFlowControlType(MethodInfo realMethodInfo, MethodInfo internalMethodInfo)
		{
			ParameterInfo[] realParameters = realMethodInfo.GetParameters();
			Type realReturnType = realMethodInfo.ReturnType;
			ParameterInfo[] internalParameters = internalMethodInfo.GetParameters();
			ParameterInfo internalLastParameter = internalParameters.LastOrDefault();
			return (realParameters.Length + 1 == internalParameters.Length &&
			        internalLastParameter?.ParameterType == realReturnType.MakeByRefType());

		}
	}
}
