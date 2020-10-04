using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class InitialValue<T> : IPropertyDefinition<T>
  {
    private readonly T value;
    private bool valueSet;

    public InitialValue(T value)
    {
      this.value = value;
    }

    public bool InvokeGetter(object _, ref T returnValue)
    {
      if (valueSet)
        return true;
      returnValue = value;
      return false;
    }

    public bool InvokeSetter(object _, T newValue)
    {
      valueSet = true;
      return true;
    }
  }
}