using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Events
{
  public class SimpleEventDefinition<T> : IDefinition
  {
    private readonly string name;

    public SimpleEventDefinition(string name)
    {
      this.name = name;
    }

    public int SortOrder => 1;

    public bool CanGenerate(MemberInfo memberInfo) =>
      memberInfo switch
      {
        EventInfo eventInfo => eventInfo.Name == name,
        _ => false,
      };

    public ICodeGenerator GetGenerator(GeneratorContext _) => new SimpleEventGenerator<T>(name);
  }
}
