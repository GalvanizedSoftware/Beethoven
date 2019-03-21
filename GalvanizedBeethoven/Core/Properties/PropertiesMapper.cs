using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using static GalvanizedSoftware.Beethoven.Core.Constants;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  public class PropertiesMapper : IEnumerable<Property>
  {
    private readonly Property[] properties;

    public PropertiesMapper(object baseObject)
    {
      properties = GetAllMembers(baseObject).ToArray();
    }

    public IEnumerator<Property> GetEnumerator()
    {
      return properties.AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private IEnumerable<Property> GetAllMembers(object baseObject)
    {
      if (baseObject == null)
        yield break;
      MethodInfo genericMethod = GetType().GetMethod(nameof(CreateProperty), StaticResolveFlags);
      Debug.Assert(genericMethod != null, nameof(genericMethod) + " != null");
      foreach (PropertyInfo propertyInfo in baseObject.GetType().GetProperties(ResolveFlags))
      {
        yield return (Property)genericMethod.
          MakeGenericMethod(propertyInfo.PropertyType).
          Invoke(GetType(), new[] { propertyInfo.Name, baseObject });
      }
    }

    private static Property CreateProperty<T>(string name, object baseObject)
    {
      return new Mapped<T>(baseObject, name).CreateMasterProperty();
    }
  }
}
