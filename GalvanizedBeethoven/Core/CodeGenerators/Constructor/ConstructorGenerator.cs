using GalvanizedSoftware.Beethoven.Extensions;
using System.Collections.Generic;
using System.Linq;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Constructor
{
  internal class ConstructorGenerator
  {
    private readonly string className;

    public ConstructorGenerator(string className)
    {
      this.className = className;
    }

    internal IEnumerable<(CodeType, string)> Generate((CodeType, string)[] code)
    {
      yield return (ConstructorSignature, $"public {className}({GetParameters(code)})");
      yield return (ConstructorSignature, "{");
      foreach ((CodeType, string) line in code.Filter(ConstructorFields))
        yield return line.Format(1);
      foreach ((CodeType, string) line in code.Filter(ConstructorCode))
        yield return line.Format(1);
      yield return (ConstructorSignature, "}");
    }

    internal static string GetParameters(IEnumerable<(CodeType, string)> lines) =>
      string.Join(", ",
        lines
          .Filter(ConstructorSignature)
          .Select(item => item.Item2));

  }
}
