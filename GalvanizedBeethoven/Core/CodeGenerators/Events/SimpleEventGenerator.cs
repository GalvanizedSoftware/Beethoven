using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using System.Collections.Generic;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;
using static GalvanizedSoftware.Beethoven.Core.Enumerable;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events
{
  internal class SimpleEventGenerator<T> : ICodeGenerator
  {
    private readonly string name;
    private static readonly string fullName = typeof(T).GetFullName();

    public SimpleEventGenerator(string name)
    {
      this.name = name;
    }

    public IEnumerable<(CodeType, string)?> Generate() => 
	    SingleEnumerable<(CodeType, string)?>((EventsCode, $@"public event {fullName} {name};"));
  }
}
