using System;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class DelegateGetter<T> : IPropertyDefinition<T>
  {
    private readonly Func<T> delegateFunc;

    public DelegateGetter(Func<T> delegateFunc)
    {
      this.delegateFunc = delegateFunc;
    }

    public bool InvokeGetter(ref T returnValue)
    {
      returnValue = delegateFunc();
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      return true;
    }
  }
}
