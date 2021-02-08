using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
	public class MainPropertyInvoker<T>
  {
    private readonly IPropertyInvoker<T>[] implementation;

    public MainPropertyInvoker(IEnumerable<IPropertyInvoker<T>> instances)
    {
      implementation = instances.ToArray();
    }

    public T InvokeGetter()
    {
      T value = default(T);
      foreach (IPropertyInvoker<T> definition in implementation)
        if (!definition.InvokeGetter(ref value))
          return value;
      return value;
    }

    public void InvokeSetter(T newValue)
    {
      foreach (IPropertyInvoker<T> definition in implementation)
        if (!definition.InvokeSetter(newValue))
          return;
    }
  }
}
