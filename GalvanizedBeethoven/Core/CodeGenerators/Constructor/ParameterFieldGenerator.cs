using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Constructor
{
  internal class ParameterFieldGenerator : ICodeGenerator<ConstructorInfo>
  {
    private readonly string fieldName;
    private readonly Type type;

    public ParameterFieldGenerator(Type type, string fieldName)
    {
      this.type = type ?? throw new NullReferenceException();
      this.fieldName = fieldName;
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      yield return $"{type.GetFullName()} {fieldName}";
      yield return $"this.{fieldName} = {fieldName};";
    }
  }
}
