using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods;

namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
	internal class LinkedDefinitions<T> : IEnumerable<IDefinition> where T : class
	{
		private readonly IDefinition[] definitions;
		private readonly MemberInfoList memberInfoList;

		internal LinkedDefinitions(IEnumerable<object> newPartDefinitions)
		{
			Type type = typeof(T);
			memberInfoList = MemberInfoListCache.Get(type);

			object[] allInstances = newPartDefinitions.ToArray();
			IDefinition[] mapped = new MappedDefinitions<T>(allInstances)
				.GetDefinitions<T>()
				.ToArray();
			allInstances = allInstances
				.Concat(mapped)
				.ToArray();
			IEnumerable<object> allObjects = allInstances
				.SelectMany(GetAll)
				.Distinct()
				.ToArray();
			definitions = allObjects
				.OfType<IDefinition>()
				.Distinct()
				.OrderBy(definition => definition.SortOrder)
				.ToArray();
			(MethodInfo methodInfo, MethodDefinition[])[] invokerMap = memberInfoList
				.MethodInfos
				.Select(FindGenerators)
				.ToArray();
			MethodFieldInvoker[] methodFieldMaps = invokerMap
				.Select(tuple => 
					new MethodFieldInvoker(memberInfoList, tuple.Item1, tuple.Item2))
				.ToArray();

			FieldMaps = allObjects
				.OfType<IFieldMaps>()
				.Concat(methodFieldMaps)
				.ToArray();

			//linkedMethodInfo = methodInfos.Single();
			//if (linkedMethodInfo?.IsGenericMethodDefinition == true)
			//	return;
			//invokerName = memberInfoList.GetMethodInvokerName(linkedMethodInfo);
			//invokerFactory = () => new RealMethodInvokerFactory(linkedMethodInfo, this);

			IInvoker[] invokers = invokerMap
				.SelectMany(
					tuple => tuple.Item2.SelectMany(definition => definition.GetInvokers(tuple.Item1)))
				.ToArray();
			allInstances
				.OfType<IMainTypeUser>()
				.SetAll(type);
		}

		private (MethodInfo methodInfo, MethodDefinition[]) FindGenerators(MethodInfo methodInfo) =>
			(methodInfo,
				definitions
					.OfType<MethodDefinition>()
					.Where(definition => definition.CanGenerate(methodInfo)).ToArray());

		public IReadOnlyCollection<IFieldMaps> FieldMaps { get; }

		public IEnumerator<IDefinition> GetEnumerator() =>
			definitions.AsEnumerable().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() =>
			GetEnumerator();

		private static IEnumerable<object> GetAll(object part)
		{
			if (part is IFieldMaps)
				yield return part;
			switch (part)
			{
				case IDefinitions definitions:
					foreach (IDefinition definition in definitions.GetDefinitions<T>())
						yield return definition;
					break;
				case IEnumerable childObjects:
					foreach (object child in childObjects)
						yield return child;
					break;
				default:
					yield return part;
					break;
			}
		}
	}
}
