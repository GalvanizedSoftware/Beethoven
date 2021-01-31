using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
	internal class LinkedDefinitions<T> : IEnumerable<IDefinition> where T : class
	{
		private readonly IDefinition[] definitions;

		internal LinkedDefinitions(IEnumerable<object> newPartDefinitions)
		{
			object[] allInstances = newPartDefinitions.ToArray();
			IDefinition[] mapped = new MappedDefinitions<T>(allInstances)
				.GetDefinitions<T>()
				.ToArray();
			allInstances = allInstances
				.Concat(mapped)
				.ToArray();
			IEnumerable<object> allObjects = allInstances
				.SelectMany(GetAll)
				.Distinct()
				.ToArray();
			GeneratorAnalyzer<T> generatorAnalyzer = new(allObjects);
			definitions = generatorAnalyzer.Definitions;
			FieldMaps = allObjects
				.OfType<IFieldMaps>()
				.Concat(generatorAnalyzer.MethodFieldMaps)
				.ToArray();

			allObjects
				.OfType<IMainTypeUser>()
				.SetAll<T>();
		}

		public IReadOnlyCollection<IFieldMaps> FieldMaps { get; }

		public IEnumerator<IDefinition> GetEnumerator() =>
			definitions.AsEnumerable().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() =>
			GetEnumerator();

		private static IEnumerable<object> GetAll(object part)
		{
			if (part is IFieldMaps)
				yield return part;
			switch (part)
			{
				case IDefinitions definitions:
					foreach (IDefinition definition in definitions.GetDefinitions<T>())
						yield return definition;
					break;
				case IEnumerable childObjects:
					foreach (object child in childObjects)
						yield return child;
					break;
				default:
					yield return part;
					break;
			}
		}
	}
}
