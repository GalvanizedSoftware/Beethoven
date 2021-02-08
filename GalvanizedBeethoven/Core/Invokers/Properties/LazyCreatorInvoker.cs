using System;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  public class LazyCreatorInvoker<T> : IPropertyInvoker<T>
  {
    private readonly Func<T> valueCreator;
    private T value;
    private bool valueCreated;
    private bool valueSet;

    public LazyCreatorInvoker(Func<T> valueCreator)
    {
      this.valueCreator = valueCreator;
    }

    public bool InvokeGetter(ref T returnValue)
    {
      if (valueSet)
        return true;
      if (!valueCreated)
        value = valueCreator();
      returnValue = value;
      valueCreated = true;
      return false;
    }

    public bool InvokeSetter(T __)
    {
      value = default(T);
      valueSet = true;
      return true;
    }
  }
}