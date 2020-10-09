using GalvanizedSoftware.Beethoven.Extensions;
using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  internal sealed class PropertyNotImplementedGenerator : ICodeGenerator
  {
    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      if (generatorContext.CodeType != CodeType.Properties)
        yield break;
      PropertyInfo propertyInfo = generatorContext?.MemberInfo as PropertyInfo;
      yield return $@"public {propertyInfo.PropertyType.GetFullName()} {propertyInfo.Name}";
      yield return "{";
      yield return $"get => throw new System.MissingMethodException();".Format(1);
      yield return $"set => throw new System.MissingMethodException();".Format(1);
      yield return "}";
    }
  }
}