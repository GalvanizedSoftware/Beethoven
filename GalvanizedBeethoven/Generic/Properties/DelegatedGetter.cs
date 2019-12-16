using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class DelegatedGetter<T> : IPropertyDefinition<T>
  {
    private readonly Func<T> delegateFunc;

    public DelegatedGetter(Func<T> delegateFunc)
    {
      this.delegateFunc = delegateFunc;
    }

    // ReSharper disable once RedundantAssignment
    public bool InvokeGetter(InstanceMap instanceMap, ref T returnValue)
    {
      returnValue = delegateFunc();
      return true;
    }

    public bool InvokeSetter(InstanceMap instanceMap, T newValue)
    {
      return true;
    }
  }
}
