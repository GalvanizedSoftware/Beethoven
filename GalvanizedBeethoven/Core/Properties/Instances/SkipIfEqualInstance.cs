using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Properties.Instances
{
  internal class SkipIfEqualInstance<T> : IPropertyInstance<T>
  {
    private T oldValue;

    public bool InvokeGetter(ref T returnValue)
    {
      return true;
    }

    public bool InvokeSetter(T newValue)
    {
      if (newValue == null)
      {
        if (oldValue == null)
          return false;
      }
      else if (newValue.Equals(oldValue))
        return false;
      oldValue = newValue;
      return true;
    }
  }
}