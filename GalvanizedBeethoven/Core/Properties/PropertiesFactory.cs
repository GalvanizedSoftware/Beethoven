using System.Collections;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Core.Interceptors;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  internal sealed class PropertiesFactory : IEnumerable<InterceptorMap>
  {
    private readonly IEnumerable<Property> properties;

    public PropertiesFactory(IEnumerable<Property> properties)
    {
      this.properties = properties;
    }

    public IEnumerator<InterceptorMap> GetEnumerator()
    {
      foreach (Property property in properties)
      {
        yield return CreatePropertyGetter(property);
        yield return CreatePropertySetter(property);
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private static InterceptorMap CreatePropertyGetter(Property property)
    {
      return new InterceptorMap(
        "get_" + property.Name, new PropertyGetInterceptor(property));
    }

    private static InterceptorMap CreatePropertySetter(Property property)
    {
      return new InterceptorMap(
        "set_" + property.Name, new PropertySetInterceptor(property));
    }
  }
}