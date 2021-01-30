using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
	internal class GenericInvokerFilter : IInvoker
	{
		private readonly IMethodMatcher methodMatcher;
		private readonly IInvoker masterInvoker;

		public GenericInvokerFilter(IMethodMatcher methodMatcher, IInvoker masterInvoker)
		{
			this.methodMatcher = methodMatcher;
			this.masterInvoker = masterInvoker;
		}

		public bool Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
			MethodInfo methodInfo)
		{
			// ReSharper disable once ConvertIfStatementToReturnStatement
			if (methodMatcher.IsMatch(methodInfo.GetParameterTypeAndNames(), genericArguments, methodInfo.ReturnType))
				return masterInvoker.Invoke(localInstance, ref returnValue, parameters, genericArguments, methodInfo);
			return true;
		}
	}
}