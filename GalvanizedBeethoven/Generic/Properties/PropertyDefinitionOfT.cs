using System;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Invokers.Factories;
using GalvanizedSoftware.Beethoven.Core.Properties.Instances;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class PropertyDefinition<T> : PropertyDefinition, IPropertyDefinition<T>
  {
    private readonly IPropertyDefinition<T>[] definitions;

    public PropertyDefinition(string name) :
      base(name, typeof(T))
    {
      definitions = Array.Empty<IPropertyDefinition<T>>();
    }

    public override int SortOrder { get; } = 1;

    public PropertyDefinition(PropertyDefinition<T> previous,
      IPropertyDefinition<T> propertyDefinition) :
      this(previous, new[] { propertyDefinition })
    {
    }

    public PropertyDefinition(PropertyDefinition<T> previous, IDefinition additionalDefinition) :
      this(previous, new IPropertyDefinition<T>[] { new InitialParameterValue<T>() }, additionalDefinition)
    {
    }


    public PropertyDefinition(PropertyDefinition<T> previous, IPropertyDefinition<T>[] propertyDefinitions,
      params IDefinition[] additionalDefinitions) :
      base(previous, additionalDefinitions)
    {
      SortOrder = previous.SortOrder;
      definitions = previous
        .definitions
        .Concat(propertyDefinitions
          .Where(definition => definition != null))
        .ToArray();
      invokerFactory = () => (object)PropertyInvokerFactory.Create<T>(definitions);
    }

    public IPropertyInstance<T> Create(object master) =>
      new MultiplePropertyInstance<T>(
        definitions.Select(definition => definition.Create(master)));
  }
}