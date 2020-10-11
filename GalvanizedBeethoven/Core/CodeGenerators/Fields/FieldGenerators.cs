using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;
using System.Linq;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
  internal class FieldGenerators : ICodeGenerator
  {
    private readonly IEnumerable<IDefinition> definitions;

    public FieldGenerators(IEnumerable<IDefinition> definitions)
    {
      this.definitions = definitions;
    }

    public IEnumerable<(CodeType, string)?> Generate(GeneratorContext generatorContext) =>
      definitions
        .GenerateCode(generatorContext.CreateLocal(FieldsCode))
        .Filter(FieldsCode)
        .Cast<(CodeType, string)?>();
  }
}
