using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using static GalvanizedSoftware.Beethoven.Core.GeneratorHelper;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
  internal class FactoryFieldGenerator : ICodeGenerator
  {
    private readonly string fieldName;
    private readonly string typeName;

    public FactoryFieldGenerator(Type type, string fieldName)
    {
      this.fieldName = fieldName;
      typeName = type.GetFullName();
    }

    public IEnumerable<(CodeType, string)?> Generate()
    {
      yield return (FieldsCode, 
        $@"{typeName} {fieldName};");
      yield return (ConstructorCode,
        $@"{fieldName} = {InstanceListName}.GetInstance<{typeName}>(""{fieldName}""); ");
    }
  }
}
