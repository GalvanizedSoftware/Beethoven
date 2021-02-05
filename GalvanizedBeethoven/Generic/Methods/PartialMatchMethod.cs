using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
	public class PartialMatchMethod : MethodDefinition
	{
		private readonly MethodInfo targetMethodInfo;
		private readonly Type mainType;
		private readonly string mainParameterName;
		private readonly object instance;

		public PartialMatchMethod(string mainName, object instance) :
			this(mainName, instance, mainName, "")
		{
		}

		public PartialMatchMethod(string mainName, object instance, string targetName) :
			this(mainName, instance, targetName, "")
		{
		}

		public PartialMatchMethod(string mainName, object instance, Type mainType, string mainParameterName) :
			this(mainName, instance, mainName, mainParameterName)
		{
			this.mainType = mainType;
		}

		public PartialMatchMethod(string mainName, object instance, string targetName, string mainParameterName) :
			this(null, mainName, instance, targetName, mainParameterName)
		{
		}

		private PartialMatchMethod(
			Type mainType, string mainName, object instance, string targetName, string mainParameterName) :
			this(mainName, mainType, instance, GetMethod(instance, targetName),mainParameterName)
		{
		}

		private PartialMatchMethod(string mainName, Type mainType,
			object instance, MethodInfo targetMethodInfo,
			string mainParameterName) :
			base(mainName, new MatchMethodNoReturn(targetMethodInfo, mainParameterName))
		{
			this.targetMethodInfo = targetMethodInfo;
			this.instance = instance;
			this.mainType = mainType;
			this.mainParameterName = mainParameterName;
		}

		private static MethodInfo GetMethod(object instance, string targetName) =>
			instance?.GetType().FindSingleMethod(targetName);

		public override IEnumerable<IInvoker> GetInvokers(MemberInfo memberInfo)
		{
			MethodInfo realMethodInfo = memberInfo as MethodInfo;
			bool isFlowControlType =
				realMethodInfo?.ReturnType != typeof(bool) &&
				targetMethodInfo.ReturnType == typeof(bool);
			if (isFlowControlType)
				yield return new PartialMatchFlowControlInvoker(targetMethodInfo, mainType, mainParameterName, instance);
			else
				yield return new PartialMatchInvoker(targetMethodInfo, mainType, mainParameterName, instance);
		}

		private static IMethodMatcher GetMethodMatcher(object instance, string targetName, string mainParameterName) =>
			new MatchMethodNoReturn(instance?.GetType(), targetName, mainParameterName);
	}
}