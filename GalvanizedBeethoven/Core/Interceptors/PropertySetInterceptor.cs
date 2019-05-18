using System;
using System.Collections.Generic;
using System.Linq;
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

    protected override void InvokeIntercept(object localInstance, Action<object> returnAction, object[] parameters) => 
      Property.InvokeSet(parameters.Single());

    public IEnumerable<TChild> Get<TChild>() => 
      objectProviderHandler.Get<TChild>();
  }
}