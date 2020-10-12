using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;
using System.Linq;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Constructor
{
  internal class ConstructorGenerator : ICodeGenerator
  {
    private readonly string className;
    private readonly IDefinition[] definitions;

    public ConstructorGenerator(string className, IEnumerable<IDefinition> definitions)
    {
      this.className = className;
      this.definitions = definitions.ToArray();
    }

    public IEnumerable<(CodeType, string)?> Generate(GeneratorContext generatorContext)
    {
      (CodeType, string)[] code = definitions.GenerateCode(generatorContext);
      string[] parameters = code
        .Filter(ConstructorSignature)
        .ToCode()
        .ToArray();
      IEnumerable<(CodeType, string)> initializers = code
         .Filter(ConstructorCode);
      yield return (ConstructorSignature, $"public {className}({string.Join(", ", parameters)})");
      yield return (ConstructorSignature, "{");
      foreach ((CodeType, string) line in initializers)
        yield return line.Format(1);
      yield return (ConstructorSignature, "}");
    }
  }
}
