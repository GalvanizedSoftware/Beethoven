using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal sealed class PropertyGetInterceptor : PropertyInterceptor
  {
    public PropertyGetInterceptor(Property property)
      :base(property)
    {
    }

    protected override void InvokeIntercept(IInvocation invocation)
    {
      invocation.ReturnValue = Property.InvokeGet();
    }
  }
}