using System.Linq;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  internal class CompositePropertyInvoker<T> : IPropertyInvoker<T>
  {
    private readonly IPropertyDefinition<T>[] definitions;

    public CompositePropertyInvoker(IPropertyDefinition<T>[] definitions)
    {
      this.definitions = definitions;
    }

    public IPropertyInvokerInstance<T> Create(object master) =>
      new CompositePropertyInvokerInstance<T>(
        definitions.Select(definition => definition.Create(master)));
  }
}
