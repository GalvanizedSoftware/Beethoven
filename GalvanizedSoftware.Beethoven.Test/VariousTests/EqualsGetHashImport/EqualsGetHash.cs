using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Test.VariousTests.EqualsGetHashImport
{
  public class EqualsGetHash<T> : IBindingParent, IEqualsGetHash where T : class
  {
    private readonly Func<T, IEnumerable<object>> valuesGetterFunc;
    private T master;

    public EqualsGetHash(Func<T, IEnumerable<object>> valuesGetterFunc, T master)
    {
	    this.valuesGetterFunc = valuesGetterFunc;
	    this.master = master as T;
    }

    public new bool Equals(object other)
    {
      return Equals((T)other);
    }

    public bool Equals(T other)
    {
      if (master is null)
        return other is null;
      bool referenceEquals = ReferenceEquals(master, other);
      bool sequenceEqual = valuesGetterFunc(master).SequenceEqual(valuesGetterFunc(other));
      return referenceEquals || sequenceEqual;
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

    public void Bind(object target)
    {
      master = target as T;
    }
  }
}
