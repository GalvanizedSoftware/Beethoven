using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal sealed class PropertySetInterceptor : PropertyInterceptor, IObjectProvider
  {
    private readonly IObjectProvider objectProviderHandler;

    public PropertySetInterceptor(PropertyDefinition propertyDefinition) :
      base(propertyDefinition)
    {
      objectProviderHandler = new ObjectProviderHandler(new[] { PropertyDefinition });
    }

    protected override void InvokeIntercept(InstanceMap instanceMap, ref object returnValue, object[] parameters) => 
      PropertyDefinition.InvokeSet(instanceMap, parameters.Single());

    public IEnumerable<TChild> Get<TChild>() => 
      objectProviderHandler.Get<TChild>();
  }
}