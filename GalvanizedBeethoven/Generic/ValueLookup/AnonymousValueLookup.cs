using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.ValueLookup
{
  public class AnonymousValueLookup : IValueLookup
  {
    private readonly object instance;
    private readonly PropertyInfo[] propertyInfos;

    public AnonymousValueLookup(object instance)
    {
      this.instance = instance;
      propertyInfos = instance?.GetType().GetProperties();
    }

    public IEnumerable<T> Lookup<T>(string name)
    {
      PropertyInfo propertyInfo = propertyInfos.FirstOrDefault(info => info.Name == name);
      if (propertyInfo == null)
        yield break;
      if (propertyInfo.PropertyType != typeof(T))
        yield break;
      yield return (T) propertyInfo.GetValue(instance);
    }
  }
}