using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
	public class MappedInvoker : IInvoker
	{
		private readonly object instance;
		private readonly MethodInfo instanceMethodInfo;

		public MappedInvoker(object instance, MethodInfo instanceMethodInfo)
		{
			this.instance = instance;
			this.instanceMethodInfo = instanceMethodInfo;
		}

		public bool Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
			MethodInfo _)
		{
			returnValue = instanceMethodInfo.Invoke(instance, parameters, genericArguments);
			return true;
		}
	}
}