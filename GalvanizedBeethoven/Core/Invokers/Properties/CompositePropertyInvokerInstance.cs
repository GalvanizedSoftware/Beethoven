﻿using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  internal class CompositePropertyInvokerInstance<T> : IPropertyInvokerInstance<T>
  {
    private readonly IPropertyInstance<T>[] implementation;

    public CompositePropertyInvokerInstance(IEnumerable<IPropertyInstance<T>> instances)
    {
      implementation = instances.ToArray();
    }

    public T InvokeGetter()
    {
      T value = default(T);
      foreach (IPropertyInstance<T> definition in implementation)
        if (!definition.InvokeGetter(ref value))
          return value;
      return value;
    }

    public void InvokeSetter(T newValue)
    {
      foreach (IPropertyInstance<T> definition in implementation)
        if (!definition.InvokeSetter(newValue))
          return;
    }
  }
}
