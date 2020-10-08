using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
  internal class FieldsGenerator : ICodeGenerator
  {
    private static readonly FieldInfo dummyFieldInfo = typeof(int).GetField(nameof(int.MaxValue));
    private readonly IEnumerable<IDefinition> definitions;

    public FieldsGenerator(IEnumerable<IDefinition> definitions)
    {
      this.definitions = definitions
        .Where(definition => definition.CanGenerate(dummyFieldInfo))
        .ToArray();
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      GeneratorContext localContext = generatorContext.CreateLocal(dummyFieldInfo, CodeType.Fields);
      return definitions
        .Select(definition => definition.GetGenerator(generatorContext))
        .SelectMany(generator => generator.Generate(localContext));
    }
  }
}
