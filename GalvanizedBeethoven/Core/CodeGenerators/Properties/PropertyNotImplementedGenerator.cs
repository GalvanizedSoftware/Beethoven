using GalvanizedSoftware.Beethoven.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  internal sealed class PropertyNotImplementedGenerator : ICodeGenerator
  {
    public IEnumerable<(CodeType, string)?> Generate(GeneratorContext generatorContext)
    {
      if (generatorContext.CodeType != PropertiesCode)
        return Enumerable.Empty<(CodeType, string)?>();
      return Generate().Select(code => ((CodeType, string)?)(PropertiesCode, code));
      IEnumerable<string> Generate()
      {
        PropertyInfo propertyInfo = generatorContext?.MemberInfo as PropertyInfo;
        yield return $@"public {propertyInfo.PropertyType.GetFullName()} {propertyInfo.Name}";
        yield return "{";
        yield return $"get => throw new System.MissingMethodException();".Format(1);
        yield return $"set => throw new System.MissingMethodException();".Format(1);
        yield return "}";
      }
    }
  }
}