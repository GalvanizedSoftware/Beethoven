using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events
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

    public ICodeGenerator GetGenerator() => new SimpleEventGenerator<T>(name);
  }
}
