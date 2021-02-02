using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using static GalvanizedSoftware.Beethoven.Core.GeneratorHelper;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.FieldInstances;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
	internal class FactoryFieldGenerator : ICodeGenerator
	{
		private readonly string fieldName;
		private readonly string typeName;
		private const string GetInstanceName = nameof(InstanceList<object>.GetInstance);

		public FactoryFieldGenerator(Type type, string fieldName)
		{
			this.fieldName = fieldName;
			typeName = type.GetFullName();
		}

		public IEnumerable<(CodeType, string)?> Generate() =>
			ConstructorCode.EnumerateCode(
				$@"{fieldName} = {InstanceListName}.{GetInstanceName}<{typeName}>(""{fieldName}"");");
	}
}
