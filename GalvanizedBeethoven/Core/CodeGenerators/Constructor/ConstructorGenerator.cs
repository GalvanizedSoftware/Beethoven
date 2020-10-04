using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
      GeneratorContext localGeneratorContext = generatorContext.CreateLocal(dummyConstructorInfo);
      string[][] codeElements = definitions
        .Select(generator => generator.GetGenerator(localGeneratorContext))
        .Select(generator => generator.Generate(localGeneratorContext).ToArray())
        .Where(element => element.Length == 2)
        .ToArray();
      string[] parameters = codeElements
        .Select(element => element[0])
        .ToArray();
      string[] initializers = codeElements
        .Select(element => element[1])
        .ToArray();
      yield return $"public {className}({string.Join(", ", parameters)})";
      yield return "{";
      foreach (string line in initializers)
        yield return line.Format(1);
      yield return "}";
    }
  }
}
