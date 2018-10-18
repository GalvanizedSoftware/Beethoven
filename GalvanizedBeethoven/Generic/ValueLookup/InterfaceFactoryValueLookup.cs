using System;
using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Generic.ValueLookup
{
  public class InterfaceFactoryValueLookup : IValueLookup
  {
    private readonly Func<Type, string, object> factoryFunc;

    public InterfaceFactoryValueLookup(Func<Type, string, object> factoryFunc)
    {
      this.factoryFunc = factoryFunc;
    }

    public IEnumerable<T> Lookup<T>(string name)
    {
      if (typeof(T).IsInterface)
        yield return (T)factoryFunc(typeof(T), name);
    }
  }
}