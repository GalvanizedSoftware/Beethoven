using Castle.DynamicProxy;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  internal sealed class PropertyGetInterceptor : PropertyInterceptor, IInterceptor
  {
    public PropertyGetInterceptor(Property property)
      :base(property)
    {
    }

    public void Intercept(IInvocation invocation)
    {
      invocation.ReturnValue = Property.InvokeGet();
    }
  }
}