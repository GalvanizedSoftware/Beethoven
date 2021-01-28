using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
	public class MappedInvoker : IInvoker
	{
		private readonly object instance;
		private readonly MethodInfo instanceMethodInfo;
		private readonly MethodInfo masterMemberInfo;

		public MappedInvoker(object instance, MethodInfo instanceMethodInfo, MethodInfo masterMemberInfo)
		{
			this.instance = instance;
			this.instanceMethodInfo = instanceMethodInfo;
			this.masterMemberInfo = masterMemberInfo;
		}

		public bool Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
			MethodInfo _)
		{
			ParameterInfo[] parameterInfos = instanceMethodInfo.GetParameters();
			ParameterInfo lastParameter = parameterInfos.LastOrDefault();
			if (parameters.Length + 1 == parameterInfos.Length &&
			    lastParameter?.ParameterType == returnValue.GetType().MakeByRefType())
			{
				object[] newParameters = parameters
					.Append(returnValue)
					.ToArray();
				instanceMethodInfo.Invoke(instance, newParameters, genericArguments);
				for (int i = 0; i < parameters.Length; i++)
					parameters[i] = newParameters[i]; // In case of ref or out variables
				returnValue = newParameters.Last();
				return true;
			}
			returnValue = instanceMethodInfo.Invoke(instance, parameters, genericArguments);
			return true;
		}
	}
}