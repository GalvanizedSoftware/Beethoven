using GalvanizedSoftware.Beethoven.Extensions;
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
    private ICodeGenerator[] codeGenerators;

    public ConstructorGenerator(string className, IEnumerable<IDefinition> definitions)
    {
      this.className = className;
      codeGenerators = definitions
        .Where(definition => definition.CanGenerate(dummyConstructorInfo))
        .Select(generator => generator.GetGenerator())
        .ToArray();
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      GeneratorContext localGeneratorContext = generatorContext.CreateLocal(dummyConstructorInfo);
      string[][] codeElements = codeGenerators
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
