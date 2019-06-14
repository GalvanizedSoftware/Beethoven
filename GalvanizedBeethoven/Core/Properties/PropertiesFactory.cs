using System.Collections;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Core.Interceptors;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  internal sealed class PropertiesFactory : IEnumerable<IInterceptorProvider>
  {
    private readonly IEnumerable<Property> properties;

    public PropertiesFactory(IEnumerable<Property> properties)
    {
      this.properties = properties;
    }

    public IEnumerator<IInterceptorProvider> GetEnumerator() => 
      properties.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}