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
  public class PropertiesMapper : IEnumerable<PropertyDefinition>
  {
    private readonly PropertyDefinition[] propertyDefinitions;
    private static readonly MethodInfo createGenericMethodInfo = 
      typeof(PropertiesMapper).GetMethod(nameof(CreatePropertyGeneric), StaticResolveFlags);
    private static readonly MethodInfo createPropertyDefinitionImportMethodInfo = 
      typeof(PropertiesMapper).GetMethod(nameof(CreatePropertyDefinitionImport), StaticResolveFlags);

    public PropertiesMapper(object baseObject)
    {
      propertyDefinitions = GetAllMembers(baseObject).ToArray();
    }

    public IEnumerator<PropertyDefinition> GetEnumerator() =>
      propertyDefinitions.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static IEnumerable<PropertyDefinition> GetAllMembers(object baseObject)
    {
      switch (baseObject)
      {
        case null:
          return Array.Empty<PropertyDefinition>();
        case DefinitionImport definitionImport:
          IParameter parameter = definitionImport.Parameter;
          PropertyInfo[] propertyInfos = parameter.Type
            .GetProperties(ResolveFlags);
          return propertyInfos
            .Select(propertyInfo =>
              (PropertyDefinition)createPropertyDefinitionImportMethodInfo.
                MakeGenericMethod(propertyInfo.PropertyType).
                Invoke(typeof(PropertiesMapper), new object[] { parameter, propertyInfo.Name }));
        default:
          return baseObject.GetType()
            .GetProperties(ResolveFlags)
            .Select(propertyInfo =>
              (PropertyDefinition)createGenericMethodInfo.
                MakeGenericMethod(propertyInfo.PropertyType).
                Invoke(typeof(PropertiesMapper), new[] { baseObject, propertyInfo.Name }));
      }
    }

    private static PropertyDefinition CreatePropertyGeneric<T>(object baseObject, string name) =>
      new Mapped<T>(baseObject, name).CreateMasterProperty();

    private static PropertyDefinition CreatePropertyDefinitionImport<T>(IParameter parameter, string name) =>
      new ParameterMapped<T>(parameter, name).CreateMasterProperty();
  }
}
