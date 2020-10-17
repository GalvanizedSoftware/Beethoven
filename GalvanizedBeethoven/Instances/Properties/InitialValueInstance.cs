using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Implementations.Properties
{
  public class InitialValueInstance<T> : IPropertyInstance<T>
  {
    private readonly T value;
    private bool valueSet;

    public InitialValueInstance(T value)
    {
      this.value = value;
    }

    public bool InvokeGetter(ref T returnValue)
    {
      if (valueSet)
        return true;
      returnValue = value;
      return false;
    }

    public bool InvokeSetter(T newValue)
    {
      valueSet = true;
      return true;
    }
  }
}