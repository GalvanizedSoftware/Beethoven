using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;

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

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      if (generatorContext.CodeType == CodeType.Fields)
        yield return $"{type.GetFullName()} {fieldName};";
    }
  }
}
