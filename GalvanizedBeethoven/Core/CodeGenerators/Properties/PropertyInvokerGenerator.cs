using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;
using static GalvanizedSoftware.Beethoven.Core.GeneratorHelper;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  internal class PropertyInvokerGenerator : ICodeGenerator
  {
	  private const string CreateName = nameof(PropertyInvokerFactory<object>.Create);
	  private const string GetInstanceName = @"GetInstance";
	  private readonly string invokerName;
    private readonly string invokerType;

    public PropertyInvokerGenerator(string invokerName, Type propertyType)
    {
      this.invokerName = invokerName;
      invokerType = typeof(PropertyInvokerFactory<>).MakeGenericType(propertyType).GetFullName();
    }

    public IEnumerable<(CodeType, string)?> Generate() =>
	    ConstructorFields.EnumerateCode(
		    $@"{invokerName} = {InstanceListName}.{GetInstanceName}<{invokerType}>(""{invokerName}"").{CreateName}(this);");
  }
}
