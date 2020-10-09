using GalvanizedSoftware.Beethoven.Extensions;
using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events
{
  internal class SimpleEventGenerator<T> : ICodeGenerator
  {
    private readonly string name;

    public SimpleEventGenerator(string name)
    {
      this.name = name;
    }

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      if (generatorContext.CodeType == CodeType.Events)
        yield return $@"public event {typeof(T).GetFullName()} {name};";
    }
  }
}
