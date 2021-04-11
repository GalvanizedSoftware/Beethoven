using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
	public class MappedFlowControlInvoker : IInvoker
	{
		private readonly object instance;
		private readonly MethodInfo instanceMethodInfo;

		public MappedFlowControlInvoker(object instance, MethodInfo instanceMethodInfo)
		{
			this.instance = instance;
			this.instanceMethodInfo = instanceMethodInfo;
		}

		public bool Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
			MethodInfo _)
		{
			object[] newParameters = parameters
				.Append(returnValue)
				.ToArray();
			bool result = (bool)instanceMethodInfo.Invoke(instance, newParameters, genericArguments);
			for (int i = 0; i < parameters.Length; i++)
				parameters[i] = newParameters[i]; // In case of ref or out variables
			returnValue = newParameters.Last();
			return result;
		}
	}
}