using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
	internal class DefaultMethodInfoInvoker : IInvoker
	{
		private readonly MethodInfo methodInfo;
		private readonly Func<MethodInfo, object[], object> mainFunc;

		public DefaultMethodInfoInvoker(MethodInfo methodInfo, Func<MethodInfo, object[], object> mainFunc)
		{
			this.methodInfo = methodInfo ?? throw new NullReferenceException();
			this.mainFunc = mainFunc ?? throw new NullReferenceException();
		}

		public bool Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
				MethodInfo _)
		{
			returnValue = mainFunc(methodInfo, parameters);
			return true;
		}
	}
}
