using Castle.DynamicProxy;

namespace GalvanizedSoftware.Beethoven.Core.Properties
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