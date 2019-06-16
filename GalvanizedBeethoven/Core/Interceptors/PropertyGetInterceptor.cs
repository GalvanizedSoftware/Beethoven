using System;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal sealed class PropertyGetInterceptor : PropertyInterceptor
  {
    public PropertyGetInterceptor(Property property)
      :base(property)
    {
    }

    protected override void InvokeIntercept(InstanceMap instanceMap, ref object returnValue, object[] parameters) => 
      returnValue = Property.InvokeGet(instanceMap);
  }
}