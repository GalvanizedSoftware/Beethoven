using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
	public class LinkedMappedMethods : IDefinitions
	{
		private readonly MethodsMapperEngine methodsMapperEngine;
		private readonly Func<object, object> creatorFunc;

		public static LinkedMappedMethods Create<TDefinition>(Func<object, TDefinition> creatorFunc) =>
			new(MethodsMapperEngine.Create<TDefinition>(), instance => creatorFunc(instance));

		private LinkedMappedMethods(MethodsMapperEngine methodsMapperEngine, Func<object, object> creatorFunc)
		{
			this.methodsMapperEngine = methodsMapperEngine;
			this.creatorFunc = creatorFunc;
		}

		public IEnumerable<IDefinition> GetDefinitions<T>() where T : class =>
			methodsMapperEngine
				.GetMethodInfos(typeof(T))
				.Select(info => new LinkedMappedMethod(info, creatorFunc));
	}
}
