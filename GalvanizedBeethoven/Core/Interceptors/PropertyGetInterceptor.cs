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

    protected override void InvokeIntercept(object localInstance, Action<object> returnAction, object[] parameters) => 
      returnAction(Property.InvokeGet());
  }
}