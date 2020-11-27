using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Generic.ConstructorParameters
{
  internal class PropertyInitializedGenerator : ICodeGenerator
  {
    private readonly string name;
    private readonly Type type;
    private readonly string parameterName;

    public PropertyInitializedGenerator(string name, Type type)
    {
      this.name = name ?? throw new NullReferenceException();
      this.type = type ?? throw new NullReferenceException();
      parameterName = $"{char.ToUpper(name[0], CultureInfo.InvariantCulture)}{name.Substring(1)}";
    }

    public IEnumerable<(CodeType, string)?> Generate()
    {
      yield return (ConstructorSignature, $"{type.GetFullName()} {parameterName}");
      yield return (ConstructorCode, $"this.{name} = {parameterName};");
    }
  }
}
