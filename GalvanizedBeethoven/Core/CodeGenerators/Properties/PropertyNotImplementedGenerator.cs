using GalvanizedSoftware.Beethoven.Extensions;
using System.Collections.Generic;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  public sealed class PropertyNotImplementedGenerator : ICodeGenerator
  {
    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      PropertyInfo propertyInfo = generatorContext?.MemberInfo as PropertyInfo;
      if (propertyInfo == null)
        yield break;
      yield return $@"public {propertyInfo.PropertyType.FullName} {propertyInfo.Name}";
      yield return "{";
      yield return $"get => throw new System.MissingMethodException();".Format(1);
      yield return $"set => throw new System.MissingMethodException();".Format(1);
      yield return "}";
    }
  }
}