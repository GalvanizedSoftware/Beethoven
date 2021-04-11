using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using System.Collections.Generic;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

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
	    EventsCode.EnumerateCode(
		    $@"public event {fullName} {name};");
  }
}
