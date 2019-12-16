using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal sealed class PropertyGetInterceptor : PropertyInterceptor
  {
    public PropertyGetInterceptor(PropertyDefinition propertyDefinition)
      :base(propertyDefinition)
    {
    }

    protected override void InvokeIntercept(InstanceMap instanceMap, ref object returnValue, object[] parameters) => 
      returnValue = PropertyDefinition.InvokeGet(instanceMap);
  }
}