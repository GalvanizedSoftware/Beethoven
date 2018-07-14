using System;
using System.Linq;
using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Test.PropertyTests
{
  class DefaultImplementation2
  {
    private readonly Dictionary<Type, object> values = new Dictionary<Type, object>();

    public void DelegatedSetter<T>(T value)
    {
      values[typeof(T)] = value;
    }

    internal object[] GetObjects()
    {
      return values.Values.ToArray();
    }
  }
}
