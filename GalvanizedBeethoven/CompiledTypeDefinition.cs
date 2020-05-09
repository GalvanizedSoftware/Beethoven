using System.Linq;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven
{
  public class CompiledTypeDefinition<T> where T : class
  {
    private readonly object[] partDefinitions;
    private readonly BeethovenFactory beethovenFactory;

    internal CompiledTypeDefinition(BeethovenFactory beethovenFactory,
      object[] partDefinitions)
    {
      this.partDefinitions = partDefinitions;
      this.beethovenFactory = beethovenFactory;
      partDefinitions.OfType<IMainTypeUser>().SetAll(typeof(T));
    }

    public T Create(params object[] parameters) =>
      beethovenFactory.Create<T>(partDefinitions, parameters);
  }
}
