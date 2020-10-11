using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Constructor
{
  internal class ParameterFieldGenerator : ICodeGenerator
  {
    private readonly string fieldName;
    private readonly Type type;

    public ParameterFieldGenerator(Type type, string fieldName)
    {
      this.type = type ?? throw new NullReferenceException();
      this.fieldName = fieldName;
    }

    public IEnumerable<(CodeType, string)?> Generate(GeneratorContext generatorContext)
    {
      yield return (ConstructorSignature, $"{type.GetFullName()} {fieldName}");
      yield return (ConstructorCode, $"this.{fieldName} = {fieldName};");
      //yield return
      //  generatorContext.CodeType switch
      //  {
      //    ConstructorSignature => (ConstructorSignature, $"{type.GetFullName()} {fieldName}"),
      //    ConstructorCode => (ConstructorCode, $"this.{fieldName} = {fieldName};"),
      //    _ => null
      //  };
    }
  }
}
