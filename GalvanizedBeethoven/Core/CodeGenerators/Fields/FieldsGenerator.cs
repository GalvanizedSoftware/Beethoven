using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
  internal class FieldsGenerator : ICodeGenerator
  {
    private readonly IEnumerable<IDefinition> definitions;

    public FieldsGenerator(IEnumerable<IDefinition> definitions)
    {
      this.definitions = definitions;
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext) =>
      definitions
        .GenerateCode(generatorContext.CreateLocal(CodeType.Fields));
  }
}
