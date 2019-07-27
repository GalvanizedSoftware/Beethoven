using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Generic.Parameters;
using GalvanizedSoftware.Beethoven.Generic.Properties;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  public sealed class Property<T> : Property, IObjectProvider, IPropertyDefinition<T>
  {
    private readonly IPropertyDefinition<T>[] definitions;
    private readonly IObjectProvider objectProviderHandler;

    public Property(string name) :
      base(name, typeof(T))
    {
      definitions = Array.Empty<IPropertyDefinition<T>>();
    }

    public Property(Property<T> previous) :
      this(previous, Array.Empty<IPropertyDefinition<T>>())
    {
    }

    public Property(Property<T> previous,
      IPropertyDefinition<T> propertyDefinition) :
      this(previous, new[] { propertyDefinition })
    {
    }

    public Property(Property<T> previous, IParameter parameter) :
      this(previous, new IPropertyDefinition<T>[] { new InitialParameterValue<T>(parameter) }, parameter)
    {
    }


    public Property(Property<T> previous, IPropertyDefinition<T>[] propertyDefinitions,
      IParameter parameter = null) :
      base(previous, parameter)
    {
      definitions = previous?
        .definitions
        .Concat(propertyDefinitions
          .Where(definition => definition != null))
        .ToArray();
      objectProviderHandler = new ObjectProviderHandler(definitions);
    }

    public IEnumerable<TChild> Get<TChild>() =>
      objectProviderHandler.Get<TChild>();

    public bool InvokeGetter(InstanceMap instanceMap, ref T returnValue)
    {
      foreach (IPropertyDefinition<T> definition in definitions)
        if (!definition.InvokeGetter(instanceMap, ref returnValue))
          return false;
      return true;
    }

    public bool InvokeSetter(InstanceMap instanceMap, T newValue)
    {
      foreach (IPropertyDefinition<T> definition in definitions)
        if (!definition.InvokeSetter(instanceMap, newValue))
          return false;
      return true;
    }

    internal override object InvokeGet(InstanceMap instanceMap)
    {
      T value = default;
      InvokeGetter(instanceMap, ref value);
      return value;
    }

    internal override void InvokeSet(InstanceMap instanceMap, object newValue) =>
      InvokeSetter(instanceMap, (T)newValue);
  }
}