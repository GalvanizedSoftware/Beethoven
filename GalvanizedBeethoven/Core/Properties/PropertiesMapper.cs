using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using static GalvanizedSoftware.Beethoven.Core.Constants;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  public class PropertiesMapper : IEnumerable<Property>
  {
    private readonly Property[] properties;
    private static readonly MethodInfo createPropertyMethodInfo;

    static PropertiesMapper()
    {
      createPropertyMethodInfo = typeof(PropertiesMapper)
        .GetMethod(nameof(CreateProperty), StaticResolveFlags);
    }

    public PropertiesMapper(object baseObject)
    {
      properties = GetAllMembers(baseObject).ToArray();
    }

    public IEnumerator<Property> GetEnumerator() => 
      properties.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private IEnumerable<Property> GetAllMembers(object baseObject)
    {
      if (baseObject == null)
        yield break;
      foreach (PropertyInfo propertyInfo in baseObject.GetType().GetProperties(ResolveFlags))
      {
        yield return (Property)createPropertyMethodInfo.
          MakeGenericMethod(propertyInfo.PropertyType).
          Invoke(GetType(), new[] { propertyInfo.Name, baseObject });
      }
    }

    private static Property CreateProperty<T>(string name, object baseObject) => 
      new Mapped<T>(baseObject, name).CreateMasterProperty();
  }
}
