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
	public abstract class MethodDefinition : DefaultDefinition, IDefinitions, IMainTypeUser
	{
		protected string invokerName;
		protected Func<object> invokerFactory;
		protected MethodInfo linkedMethodInfo;

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

		public virtual void Set(Type setMainType)
		{
			//MemberInfoList memberInfoList = MemberInfoListCache.Get(setMainType);
			//MethodInfo[] methodInfos = memberInfoList
			//	.MethodInfos
			//	.Where(CanGenerate)
			//	.ToArray();
			//if (methodInfos.Length == 0)
			//	return;
			//linkedMethodInfo = methodInfos.Single();
			//invokerName = memberInfoList.GetMethodInvokerName(linkedMethodInfo);
			//invokerFactory = () => new RealMethodInvokerFactory(linkedMethodInfo, this);
		}

		public override bool CanGenerate(MemberInfo memberInfo) =>
			MethodMatcher.IsMatchIgnoreGeneric(memberInfo as MethodInfo, Name);

		public override ICodeGenerator GetGenerator(GeneratorContext _) => null;

		public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo) =>
			Enumerable.Empty<IInvoker>();

		//public virtual IEnumerable<(string, object)> GetFields()
		//{
		//	if (invokerName != null)
		//		yield return (invokerName, invokerFactory());
		//}

		public IEnumerable<IDefinition> GetDefinitions<T>() where T : class
		{
			Set(typeof(T));
			yield return this;
		}
	}
}
