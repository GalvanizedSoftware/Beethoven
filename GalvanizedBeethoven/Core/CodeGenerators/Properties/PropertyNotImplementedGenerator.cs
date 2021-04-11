using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using System.Collections.Generic;
using System.Reflection;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
	internal sealed class PropertyNotImplementedGenerator : ICodeGenerator
	{
		private readonly PropertyInfo propertyInfo;

		public PropertyNotImplementedGenerator(PropertyInfo propertyInfo)
		{
			this.propertyInfo = propertyInfo;
		}

		public IEnumerable<(CodeType, string)?> Generate() =>
			PropertiesCode.EnumerateCode
			(
				$@"public {propertyInfo.PropertyType.GetFullName()} {propertyInfo.Name}",
				"{",
				"get => throw new System.MissingMethodException();".Format(1),
				"set => throw new System.MissingMethodException();".Format(1),
				"}"
			);
	}
}