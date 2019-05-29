using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  internal class SkipIfEqual<T> : IPropertyDefinition<T>
  {
    private T oldValue;

    public bool InvokeGetter(InstanceMap instanceMap, ref T returnValue)
    {
      return true;
    }

    public bool InvokeSetter(InstanceMap instanceMap, T newValue)
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