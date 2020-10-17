using System;
using System.Linq;
using GalvanizedSoftware.Beethoven.Implementations.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public sealed class PropertyDefinition<T> : PropertyDefinition, IPropertyDefinition<T>
  {
    private readonly IPropertyDefinition<T>[] definitions;

    public PropertyDefinition(string name) :
      base(name, typeof(T))
    {
      definitions = Array.Empty<IPropertyDefinition<T>>();
    }

    public PropertyDefinition(PropertyDefinition<T> previous) :
      this(previous, Array.Empty<IPropertyDefinition<T>>())
    {
    }

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
      definitions = previous?
        .definitions
        .Concat(propertyDefinitions
          .Where(definition => definition != null))
        .ToArray();
    }

    internal override object[] Definitions => definitions.OfType<object>().ToArray();

    public IPropertyInstance<T> CreateInstance(object master) =>
      new MultiplePropertyInstance<T>(
        definitions.Select(definition => definition.CreateInstance(master)));
  }
}