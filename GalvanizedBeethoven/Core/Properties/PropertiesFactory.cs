using System.Collections;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Core.Interceptors;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  internal sealed class PropertiesFactory : IEnumerable<IInterceptorProvider>
  {
    private readonly IEnumerable<PropertyDefinition> propertyDefinitions;

    public PropertiesFactory(IEnumerable<PropertyDefinition> propertyDefinitions)
    {
      this.propertyDefinitions = propertyDefinitions;
    }

    public IEnumerator<IInterceptorProvider> GetEnumerator() => 
      propertyDefinitions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}