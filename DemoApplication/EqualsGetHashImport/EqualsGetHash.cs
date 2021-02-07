using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.DemoApp.EqualsGetHashImport
{
  public class EqualsGetHash<T> : IEqualsGetHash where T : class
  {
    private readonly Func<T, IEnumerable<object>> valuesGetterFunc;
    private readonly T master;

    public EqualsGetHash(Func<T, IEnumerable<object>> valuesGetterFunc, T master)
    {
	    this.valuesGetterFunc = valuesGetterFunc;
	    this.master = master;
    }

    public new bool Equals(object other) => 
	    Equals((T)other);

    public bool Equals(T other)
    {
      if (master is null)
        return ReferenceEquals(null, other);
      return ReferenceEquals(master, other) || valuesGetterFunc(master).SequenceEqual(valuesGetterFunc(other));
    }

    public new int GetHashCode()
    {
      if (master is null)
        return 0.GetHashCode();
      unchecked
      {
        return valuesGetterFunc(master)
          .Aggregate(17, (hash, obj) => hash * 23 + obj?.GetHashCode() ?? 0);
      }
    }
  }
}
