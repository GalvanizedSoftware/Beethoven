using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Methods
{
	public class PartialMatchInvoker : IInvoker
	{
		private readonly MethodInfo methodInfo;
		private readonly bool hasReturnType;
		private readonly (Type, string)[] localParameters;
		private readonly Type mainType;
		private readonly string mainParameterName;
		private readonly object instance;

		public PartialMatchInvoker(MethodInfo methodInfo, Type mainType,
			string mainParameterName, object instance)
		{
			this.methodInfo = methodInfo;
			this.instance = instance;
			localParameters = methodInfo.GetParameterTypeAndNames();
			hasReturnType = methodInfo.HasReturnType();
			this.mainType = mainType;
			this.mainParameterName = mainParameterName;
		}

		public bool Invoke(object localInstance, ref object returnValue, object[] parameters, Type[] genericArguments,
			MethodInfo masterMethodInfo)
		{
			(Type, string)[] masterParameters = masterMethodInfo
				.GetParameterTypeAndNames()
				.AppendReturnValue(masterMethodInfo?.ReturnType)
				.Append((mainType, mainParameterName))
				.ToArray();
			int[] indexes = localParameters
				.Select(item => Array.IndexOf(masterParameters, item))
				.ToArray();
			object[] inputParameters = GetInputParameters(localInstance, parameters, masterParameters.Length);
			object[] localParameterValues = indexes
				.Select(index => inputParameters[index])
				.ToArray();
			object invokeResult = methodInfo.Invoke(instance, localParameterValues, genericArguments);
			(object, int)[] mappedValues = localParameterValues
				.Zip(indexes, (value, index) => (value, index))
				.ToArray();
			 (object, int)? returnValueParameter = mappedValues
				.Where(tuple => tuple.Item2 == parameters.Length)
				.Cast<(object, int)?>()
				.FirstOrDefault();
			if (hasReturnType)
				returnValue = invokeResult;
			else if (returnValueParameter != null) 
				returnValue = returnValueParameter.Value.Item1;

			foreach ((object value, int index) in mappedValues)
				SetIfValid(parameters, index, value, masterParameters);
			return true;
		}

		private static object[] GetInputParameters(object localInstance, object[] parameters, int length)
		{
			if (parameters == null || length == 0)
				return Array.Empty<object>();
			object[] returnValues = new object[length];
			for (int i = 0; i < parameters.Length; i++)
				returnValues[i] = parameters[i];
			returnValues[length - 1] = localInstance;
			return returnValues;
		}

		private static void SetIfValid(object[] parameters, int index, object value, (Type, string)[] masterParameters)
		{
			if (index >= 0 && index < parameters.Length && masterParameters[index].Item1.IsByRefence())
				parameters[index] = value;
		}
	}
}