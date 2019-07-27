using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Parameters;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using static GalvanizedSoftware.Beethoven.Core.Constants;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  public class PropertiesMapper : IEnumerable<Property>
  {
    private readonly Property[] properties;
    private static readonly MethodInfo createGenericMethodInfo = 
      typeof(PropertiesMapper).GetMethod(nameof(CreatePropertyGeneric), StaticResolveFlags);
    private static readonly MethodInfo createPropertyDefinitionImportMethodInfo = 
      typeof(PropertiesMapper).GetMethod(nameof(CreatePropertyDefinitionImport), StaticResolveFlags);

    public PropertiesMapper(object baseObject)
    {
      properties = GetAllMembers(baseObject).ToArray();
    }

    public IEnumerator<Property> GetEnumerator() =>
      properties.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static IEnumerable<Property> GetAllMembers(object baseObject)
    {
      switch (baseObject)
      {
        case null:
          return Array.Empty<Property>();
        case DefinitionImport definitionImport:
          IParameter parameter = definitionImport.Parameter;
          PropertyInfo[] propertyInfos = parameter.Type
            .GetProperties(ResolveFlags);
          return propertyInfos
            .Select(propertyInfo =>
              (Property)createPropertyDefinitionImportMethodInfo.
                MakeGenericMethod(propertyInfo.PropertyType).
                Invoke(typeof(PropertiesMapper), new object[] { parameter, propertyInfo.Name }));
        default:
          return baseObject.GetType()
            .GetProperties(ResolveFlags)
            .Select(propertyInfo =>
              (Property)createGenericMethodInfo.
                MakeGenericMethod(propertyInfo.PropertyType).
                Invoke(typeof(PropertiesMapper), new[] { baseObject, propertyInfo.Name }));
      }
    }

    private static Property CreatePropertyGeneric<T>(object baseObject, string name) =>
      new Mapped<T>(baseObject, name).CreateMasterProperty();

    private static Property CreatePropertyDefinitionImport<T>(IParameter parameter, string name) =>
      new ParameterMapped<T>(parameter, name).CreateMasterProperty();
  }
}
