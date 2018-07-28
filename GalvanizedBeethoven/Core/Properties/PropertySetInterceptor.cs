using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  internal sealed class PropertySetInterceptor : PropertyInterceptor, IInterceptor, IObjectProvider
  {
    private readonly IObjectProvider objectProviderHandler;

    public PropertySetInterceptor(Property property) :
      base(property)
    {
      objectProviderHandler = new ObjectProviderHandler(new[] { Property });
    }

    public void Intercept(IInvocation invocation)
    {
      Property.InvokeSet(invocation.Arguments.Single());
    }

    public IEnumerable<TChild> Get<TChild>()
    {
      return objectProviderHandler.Get<TChild>();
    }
  }
}