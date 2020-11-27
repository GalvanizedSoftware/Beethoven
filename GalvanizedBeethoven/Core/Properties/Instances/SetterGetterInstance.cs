using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Properties.Instances
{
  public class SetterGetterInstance<T> : IPropertyInstance<T>
  {
    private T value;

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