using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
  internal class SimpleFieldGenerator : ICodeGenerator
  {
    private readonly Type type;
    private readonly string fieldName;

    public SimpleFieldGenerator(Type type, string fieldName)
    {
      this.type = type ?? throw new NullReferenceException();
      this.fieldName = fieldName;
    }

    public IEnumerable<(CodeType, string)?> Generate(GeneratorContext generatorContext)
    {
      yield return (FieldsCode, $"{type.GetFullName()} {fieldName};");
    }
  }
}
