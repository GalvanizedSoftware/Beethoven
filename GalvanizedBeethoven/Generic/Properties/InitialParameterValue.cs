using System;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class InitialParameterValue<T> : IPropertyDefinition<T>
  {
    private readonly IParameter parameter;
    private bool valueSet;

    public InitialParameterValue(IParameter parameter)
    {
      this.parameter = parameter ?? throw new NullReferenceException();
      if (parameter.Type != typeof(T))
        throw new InvalidCastException();
    }

    public bool InvokeGetter(InstanceMap instanceMap, ref T returnValue)
    {
      if (valueSet)
        return true;
      returnValue = (T) instanceMap?.GetLocal(parameter);
      return false;
    }

    public bool InvokeSetter(InstanceMap instanceMap, T newValue)
    {
      valueSet = true;
      return true;
    }
  }
}