using Castle.DynamicProxy;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  internal sealed class PropertyGetInterceptor : IInterceptor
  {
    private readonly Property property;

    public PropertyGetInterceptor(Property property)
    {
      this.property = property;
    }

    public void Intercept(IInvocation invocation)
    {
      invocation.ReturnValue = property.InvokeGet();
    }
  }
}