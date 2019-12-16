using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal sealed class ObjectProviderHandler : IObjectProvider
  {
    private readonly List<IObjectProvider> objectProviders = new List<IObjectProvider>();
    private readonly object[] objects;

    public ObjectProviderHandler(IEnumerable<object> objects)
    {
      this.objects = objects.ToArray();
      objectProviders.AddRange(this.objects.OfType<IObjectProvider>());
      objectProviders.AddRange(GetChildren(objectProviders));
    }

    public IEnumerable<TChild> Get<TChild>()
    {
      return objects.OfType<TChild>().Concat(
        objectProviders.SelectMany(objectProvider => objectProvider.Get<TChild>()));
    }

    private static IEnumerable<IObjectProvider> GetChildren(IEnumerable<IObjectProvider> objectProviders)
    {
      IObjectProvider[] children = objectProviders.SelectMany(
        objectProvider => objectProvider.Get<IObjectProvider>()).ToArray();
      if (children.Length == 0)
        return Array.Empty<IObjectProvider>();
      return children.Concat(GetChildren(children));
    }
  }
}