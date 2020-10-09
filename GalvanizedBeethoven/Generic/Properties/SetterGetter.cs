using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class SetterGetter<T> : IPropertyDefinition<T>
  {
    private T value;

    public bool InvokeGetter(object _, ref T returnValue)
    {
      returnValue = value;
      return true;
    }

    public bool InvokeSetter(object _, T newValue)
    {
      value = newValue;
      return true;
    }
  }
}