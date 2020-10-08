using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Constructor
{
  internal class ConstructorGenerator : ICodeGenerator
  {
    private static readonly ConstructorInfo dummyConstructorInfo = typeof(object).GetConstructor(Array.Empty<Type>());
    private readonly string className;
    private readonly IDefinition[] definitions;

    public ConstructorGenerator(string className, IEnumerable<IDefinition> definitions)
    {
      this.className = className;
      this.definitions = definitions
        .Where(definition => definition.CanGenerate(dummyConstructorInfo))
        .ToArray();
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      string[] parameters = definitions.GenerateCode(
        generatorContext.CreateLocal(dummyConstructorInfo, ConstructorSignature));
      string[] initializers = definitions.GenerateCode(
        generatorContext.CreateLocal(dummyConstructorInfo, ConstructorCode));
      yield return $"public {className}({string.Join(", ", parameters)})";
      yield return "{";
      foreach (string line in initializers)
        yield return line.Format(1);
      yield return "}";
    }
  }
}
