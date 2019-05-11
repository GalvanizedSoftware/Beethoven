using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal sealed class PropertySetInterceptor : PropertyInterceptor, IObjectProvider
  {
    private readonly IObjectProvider objectProviderHandler;

    public PropertySetInterceptor(Property property) :
      base(property)
    {
      objectProviderHandler = new ObjectProviderHandler(new[] { Property });
    }

    protected override void InvokeIntercept(IInvocation invocation)
    {
      Property.InvokeSet(invocation.Arguments.Single());
    }

    public IEnumerable<TChild> Get<TChild>()
    {
      return objectProviderHandler.Get<TChild>();
    }
  }
}