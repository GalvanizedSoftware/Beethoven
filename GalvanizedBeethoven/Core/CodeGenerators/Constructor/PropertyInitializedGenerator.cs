using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace GalvanizedSoftware.Beethoven.Generic.ConstructorParameters
{
  internal class PropertyInitializedGenerator : ICodeGenerator
  {
    private readonly string name;
    private readonly Type type;

    public PropertyInitializedGenerator(string name, Type type)
    {
      this.name = name;
      this.type = type ?? throw new NullReferenceException();
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      string parameterName = $"{char.ToUpper(name[0], CultureInfo.InvariantCulture)}{name.Substring(1)}";
      yield return $"{type.GetFullName()} {parameterName}";
      yield return $"this.{name} = {parameterName};";
    }
  }
}
