using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
  internal class SimpleFieldGenerator : ICodeGenerator<FieldInfo>
  {
    private readonly Type type;
    private readonly string fieldName;

    public SimpleFieldGenerator(Type type, string fieldName)
    {
      this.type = type ?? throw new NullReferenceException();
      this.fieldName = fieldName;
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      yield return $"{type.GetFullName()} {fieldName};";
    }
  }
}
