using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
	public class PropertyInvokerFactory<T>
  {
    private readonly IPropertyDefinition<T>[] definitions;

    public static PropertyInvokerFactory<T> Create(IEnumerable<object> definitions) =>
	    new(definitions
		    .OfType<IPropertyDefinition<T>>()
		    .ToArray());

    public PropertyInvokerFactory(IPropertyDefinition<T>[] definitions)
    {
      this.definitions = definitions;
    }

    public MainPropertyInvoker<T> Create(object master) =>
      new(definitions.Select(definition => definition.Create(master)));
  }
}
