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

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      string[] parameters = definitions.GenerateCode(
        generatorContext.CreateLocal(ConstructorSignature));
      string[] initializers = definitions.GenerateCode(
        generatorContext.CreateLocal(ConstructorCode));
      yield return $"public {className}({string.Join(", ", parameters)})";
      yield return "{";
      foreach (string line in initializers)
        yield return line.Format(1);
      yield return "}";
    }
  }
}
