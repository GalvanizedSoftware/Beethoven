﻿using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods;
using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
	internal class PropertyGenerator : ICodeGenerator
	{
		private readonly Type propertyType;
		private readonly string invokerName;
		private readonly string propertyInfoName;
		private readonly Type invokerInstanceType;
		private const string InvokeGetterName = nameof(IPropertyInvokerInstance<object>.InvokeGetter);
		private const string InvokeSetterName = nameof(IPropertyInvokerInstance<object>.InvokeSetter);

		internal PropertyGenerator(PropertyInfo propertyInfo)
		{
			propertyType = propertyInfo.PropertyType;
			propertyInfoName = propertyInfo.Name;
			invokerName = $"invoker{propertyInfoName}";
			invokerInstanceType = typeof(IPropertyInvokerInstance<>).MakeGenericType(propertyType);
		}

		public IEnumerable<(CodeType, string)?> Generate()
		{
			CodeGeneratorList invokerGenerators = new
			(
				new FieldDeclarationGenerator(invokerInstanceType, invokerName),
				new PropertyInvokerGenerator(invokerName, propertyType)
			);
			return invokerGenerators.Generate()
				.Concat(
					PropertiesCode.EnumerateCode
					(
						$@"public {propertyType.GetFullName()} {propertyInfoName}",
						"{",
						$"get => {invokerName}.{InvokeGetterName}();".Format(1),
						$"set => {invokerName}.{InvokeSetterName}(value);".Format(1),
			      "}"
					));
		}
  }
}
