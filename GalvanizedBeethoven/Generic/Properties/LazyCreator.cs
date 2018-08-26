using GalvanizedSoftware.Beethoven.Core.Properties;
using System;

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

    public bool InvokeSetter(T newValue)
    {
      value = default(T);
      valueSet = true;
      return true;
    }

    public static LazyCreator<T> CreateIfMatch(Type type, Func<object> creatorFunc)
    {
      return type != typeof(T) ? null : new LazyCreator<T>(() => (T)creatorFunc());
    }
  }
}