using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
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
    private readonly string parameterName;

    public ParameterFieldGenerator(Type type, string fieldName, string parameterName = null)
    {
      this.type = type ?? throw new NullReferenceException();
      this.fieldName = fieldName ?? throw new NullReferenceException();
      this.parameterName = parameterName ?? fieldName;
    }

    public IEnumerable<(CodeType, string)?> Generate()
    {
      yield return (ConstructorSignature, $"{type.GetFullName()} {parameterName}");
      yield return (ConstructorCode, $"this.{fieldName} = {parameterName};");
    }
  }
}
