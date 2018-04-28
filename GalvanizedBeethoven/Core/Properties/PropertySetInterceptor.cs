using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  internal sealed class PropertySetInterceptor : IInterceptor, IObjectProvider
  {
    private readonly IObjectProvider objectProviderHandler;
    private readonly Property property;

    public PropertySetInterceptor(Property property)
    {
      this.property = property;
      objectProviderHandler = new ObjectProviderHandler(new[] {property});
    }

    public void Intercept(IInvocation invocation)
    {
      property.InvokeSet(invocation.Arguments.Single());
    }

    public IEnumerable<TChild> Get<TChild>()
    {
      return objectProviderHandler.Get<TChild>();
    }
  }
}