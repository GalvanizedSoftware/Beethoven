using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class SetterGetter<T> : IPropertyDefinition<T>
  {
    private T value;

    // ReSharper disable once RedundantAssignment
    public bool InvokeGetter(ref T returnValue)
    {
      returnValue = value;
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      value = newValue;
      return true;
    }
  }
}