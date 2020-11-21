using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  internal sealed class PropertyNotImplementedGenerator : ICodeGenerator
  {
    readonly PropertyInfo propertyInfo;

    public PropertyNotImplementedGenerator(PropertyInfo propertyInfo)
    {
      this.propertyInfo = propertyInfo;
    }

    public IEnumerable<(CodeType, string)?> Generate()
    {
      return Generate().Select(code => ((CodeType, string)?)(PropertiesCode, code));
      IEnumerable<string> Generate()
      {
        yield return $@"public {propertyInfo.PropertyType.GetFullName()} {propertyInfo.Name}";
        yield return "{";
        yield return $"get => throw new System.MissingMethodException();".Format(1);
        yield return $"set => throw new System.MissingMethodException();".Format(1);
        yield return "}";
      }
    }
  }
}