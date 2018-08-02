using System;
using Castle.DynamicProxy;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  internal abstract class PropertyInterceptor : IInterceptor
  {
    internal Property Property { get; }

    protected PropertyInterceptor(Property property)
    {
      Property = property;
    }

    protected abstract void InvokeIntercept(IInvocation invocation);

    public void Intercept(IInvocation invocation)
    {
      if (!Property.IsMatch(invocation.Method))
        throw new NotImplementedException();
      InvokeIntercept(invocation);
    }
  }
}