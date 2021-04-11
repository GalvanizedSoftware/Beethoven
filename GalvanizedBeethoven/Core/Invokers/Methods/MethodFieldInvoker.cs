using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Definitions;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
	internal sealed class MethodFieldInvoker : IFieldMaps, IEnumerable<IInvoker>
	{
		private readonly string invokerName;
		private readonly Func<object> invokerFactory;
		private readonly IInvoker[] invokers;

		internal MethodFieldInvoker(MemberInfoList memberInfoList, MethodInfo methodInfo, MethodDefinition[] definitions)
		{
			invokers = definitions
				.SelectMany(definition => GetInvokers(methodInfo, definition))
				.ToArray();
			invokerName = memberInfoList.GetMethodInvokerName(methodInfo);
			invokerFactory = () => new RealMethodInvokerFactory(methodInfo, this);
		}

		private static IEnumerable<IInvoker> GetInvokers(MethodInfo methodInfo, MethodDefinition definition) =>
			!methodInfo.IsGenericMethod ?
				definition.GetInvokers(methodInfo) :
				definition
					.GetInvokers(methodInfo)
					.Select(masterInvoker => new GenericInvokerFilter(definition.MethodMatcher, masterInvoker))
					.ToArray();

		public IEnumerable<(string, object)> GetFields()
		{
			yield return (invokerName, invokerFactory());
		}

		public IEnumerator<IInvoker> GetEnumerator() =>
			invokers.AsEnumerable().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() =>
			GetEnumerator();
	}
}
