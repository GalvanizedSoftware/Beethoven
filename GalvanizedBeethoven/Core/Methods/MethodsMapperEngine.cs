using System;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
	internal class MethodsMapperEngine
	{
		private readonly MethodInfo[] interfaceMethods;

		public static MethodsMapperEngine Create<T>() =>
			new(typeof(T));

		private MethodsMapperEngine(Type mainType)
		{
			interfaceMethods = mainType.GetAllMethodsAndInherited().ToArray();
		}

		public IEnumerable<MappedMethod> GenerateMappings(object baseObject, Type baseType) =>
			GetMethodInfos(baseType)
				.Select(methodInfo => new MappedMethod(methodInfo, baseObject))
				.ToArray();

		public IEnumerable<MethodInfo> GetMethodInfos(Type baseType) =>
			baseType
				.GetAllTypes()
				.SelectMany(type => type.GetNotSpecialMethods())
				.Intersect(interfaceMethods, new ExactMethodComparer());
	}
}