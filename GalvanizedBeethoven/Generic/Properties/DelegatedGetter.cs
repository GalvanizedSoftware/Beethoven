using System;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class DelegatedGetter<T> : IPropertyDefinition<T>
  {
    private readonly Func<T> delegateFunc;

    public DelegatedGetter(Func<T> delegateFunc)
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
