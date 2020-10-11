using GalvanizedSoftware.Beethoven.Extensions;
using System.Collections.Generic;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events
{
  internal class SimpleEventGenerator<T> : ICodeGenerator
  {
    private readonly string name;

    public SimpleEventGenerator(string name)
    {
      this.name = name;
    }

    public IEnumerable<(CodeType, string)?> Generate(GeneratorContext generatorContext)
    {
      yield return (EventsCode, $@"public event {typeof(T).GetFullName()} {name};");
    }
  }
}
