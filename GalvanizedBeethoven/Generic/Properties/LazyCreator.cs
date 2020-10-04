using System;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class LazyCreator<T> : IPropertyDefinition<T>
  {
    private readonly Func<T> valueCreator;
    private T value;
    private bool valueCreated;
    private bool valueSet;

    public LazyCreator(Func<T> valueCreator)
    {
      this.valueCreator = valueCreator;
    }

    public bool InvokeGetter(object _, ref T returnValue)
    {
      if (valueSet)
        return true;
      if (!valueCreated)
        value = valueCreator();
      returnValue = value;
      valueCreated = true;
      return false;
    }

    public bool InvokeSetter(object _, T __)
    {
      value = default(T);
      valueSet = true;
      return true;
    }
  }
}