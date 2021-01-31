using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
	internal class GeneratorAnalyzer<T> where T : class
	{
		private static readonly MemberInfoList memberInfoList = MemberInfoListCache.Get(typeof(T));


		public GeneratorAnalyzer(IEnumerable<object> allObjects)
		{
			Definitions = allObjects
				.OfType<IDefinition>()
				.Distinct()
				.OrderBy(definition => definition.SortOrder)
				.ToArray();
			(MethodInfo methodInfo, MethodDefinition[])[] invokerMap = memberInfoList
				.MethodInfos
				.Select(FindGenerators)
				.ToArray();
			MethodFieldMaps = invokerMap
				.Select(tuple =>
					new MethodFieldInvoker(memberInfoList, tuple.Item1, tuple.Item2))
				.ToArray();
		}

		public IDefinition[] Definitions { get; }

		public MethodFieldInvoker[] MethodFieldMaps { get; }

		private (MethodInfo methodInfo, MethodDefinition[]) FindGenerators(MethodInfo methodInfo) =>
			(methodInfo, GetMethodDefinitions(methodInfo).ToArray());

		private IEnumerable<MethodDefinition> GetMethodDefinitions(MethodInfo methodInfo)
		{
			MethodDefinition[] methodDefinitions = Definitions
				.OfType<MethodDefinition>()
				.Where(definition => definition.CanGenerate(methodInfo))
				.ToArray();
			if (methodDefinitions.Length == 0)
				return new MethodDefinition[] { new NotImplementedMethod(methodInfo) };
			MethodDefinition[] sortOrder1 = methodDefinitions
				.Where(definition => definition.SortOrder == 1)
				.ToArray();
			MethodDefinition[] sortOrder2 = methodDefinitions
				.Where(definition => definition.SortOrder == 2)
				.ToArray();
			if (sortOrder1.Length > 0)
				return sortOrder1;
			if (sortOrder2.Length == 1)
				return sortOrder2;
			return methodDefinitions
				.Append(sortOrder2.FirstOrDefault());
		}
	}
}
