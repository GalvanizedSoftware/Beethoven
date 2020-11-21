using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Invokers
{
  internal class CompositePropertyInvoker<T> : IPropertyInvoker<T>
  {
    private readonly IPropertyDefinition<T>[] definitions;

    public CompositePropertyInvoker(IPropertyDefinition<T>[] definitions)
    {
      this.definitions = definitions;
    }

    public IPropertyInvokerInstance<T> CreateInstance(object master) =>
      new CompositePropertyInvokerInstance<T>(
        definitions.Select(definition => definition.CreateInstance(master)));
  }
}
