using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using static GalvanizedSoftware.Beethoven.Core.ReflectionConstants;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  internal class PropertiesMapper : IEnumerable<PropertyDefinition>, IEnumerable<ICodeGenerator>
  {
    private readonly PropertyDefinition[] propertyDefinitions;
    private static readonly MethodInfo createGenericMethodInfo =
      typeof(PropertiesMapper).GetMethod(nameof(CreatePropertyGeneric), StaticResolveFlags);

    public PropertiesMapper(Type type, object baseObject)
    {
      if (type != null && baseObject != null)
        propertyDefinitions = GetAllMembers(type, baseObject).ToArray();
    }

    public IEnumerator<PropertyDefinition> GetEnumerator() =>
      propertyDefinitions.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static IEnumerable<PropertyDefinition> GetAllMembers(Type type, object baseObject)
    {
      IEnumerable<PropertyInfo> baseProperties = baseObject
        .GetType()
        .GetAllProperties();
      return type.GetAllProperties()
        .Intersect(baseProperties, new ExactPropertyComparer())
        .Select(propertyInfo => (PropertyDefinition)
          createGenericMethodInfo
            .MakeGenericMethod(propertyInfo.PropertyType)
            .Invoke(typeof(PropertiesMapper), new[] { baseObject, propertyInfo.Name }));
    }

    private static PropertyDefinition CreatePropertyGeneric<T>(object baseObject, string name) =>
      new Mapped<T>(baseObject, name).CreateMasterProperty();

    IEnumerator<ICodeGenerator> IEnumerable<ICodeGenerator>.GetEnumerator() =>
       propertyDefinitions.OfType<ICodeGenerator>().GetEnumerator();
  }
}
