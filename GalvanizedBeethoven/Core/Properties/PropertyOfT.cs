using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  public sealed class Property<T> : Property, IPropertyDefinition<T>, IObjectProvider
  {
    private readonly IPropertyDefinition<T>[] definitions;
    private readonly IObjectProvider objectProviderHandler;

    public Property(string name) :
      base(name)
    {
      definitions = new IPropertyDefinition<T>[0];
    }

    public Property(Property<T> previous, params IPropertyDefinition<T>[] propertyDefinitions) :
      base(previous)
    {
      definitions = previous.definitions.Concat(propertyDefinitions).ToArray();
      objectProviderHandler = new ObjectProviderHandler(definitions);
    }

    public IEnumerable<TChild> Get<TChild>()
    {
      return objectProviderHandler.Get<TChild>();
    }

    public bool InvokeGetter(ref T returnValue)
    {
      foreach (IPropertyDefinition<T> definition in definitions)
        if (!definition.InvokeGetter(ref returnValue))
          return false;
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      foreach (IPropertyDefinition<T> definition in definitions)
        if (!definition.InvokeSetter(newValue))
          return false;
      return true;
    }

    public override Type PropertyType { get; } = typeof(T);

    internal override object InvokeGet()
    {
      T value = default(T);
      if (InvokeGetter(ref value))
        return value;
      throw new NotImplementedException();
    }

    internal override void InvokeSet(object newValue)
    {
      InvokeSetter((T)newValue);
    }
  }
}