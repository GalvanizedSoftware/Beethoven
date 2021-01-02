using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Events
{
  public class SimpleEventDefinition<T> : DefaultDefinition
  {
    private readonly string name;

    public SimpleEventDefinition(string name)
    {
      this.name = name;
    }

    public override bool CanGenerate(MemberInfo memberInfo) =>

      (memberInfo as EventInfo)?.Name == name;
    public override ICodeGenerator GetGenerator(GeneratorContext _) => new SimpleEventGenerator<T>(name);
  }
}
