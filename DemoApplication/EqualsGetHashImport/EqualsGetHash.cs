using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Binding;

namespace GalvanizedSoftware.Beethoven.DemoApp.EqualsGetHashImport
{
  public class EqualsGetHash<T> : IBindingParent, IEqualsGetHash where T : class
  {
    private readonly Func<T, IEnumerable<object>> valuesGetterFunc;
    private T master;

    public EqualsGetHash(Func<T, IEnumerable<object>> valuesGetterFunc)
    {
      this.valuesGetterFunc = valuesGetterFunc;
    }

    public new bool Equals(object other)
    {
      return Equals((T)other);
    }

    public bool Equals(T other)
    {
      if (master is null)
        return ReferenceEquals(null, other);
      return ReferenceEquals(master, other) || valuesGetterFunc(master).SequenceEqual(valuesGetterFunc((T)other));
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
