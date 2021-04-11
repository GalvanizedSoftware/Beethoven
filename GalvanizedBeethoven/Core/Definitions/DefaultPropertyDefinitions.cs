using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
  public class DefaultPropertyDefinitions
  {
    private readonly Func<Type, string, object>[] creators;

    internal DefaultPropertyDefinitions(Func<Type, string, object>[] creators)
    {
      this.creators = creators;
    }

    internal PropertyDefinition Create(PropertyInfo propertyInfo) =>
      (PropertyDefinition)createMethodInfo.Invoke(this, new object[] { propertyInfo.Name }, new[] { propertyInfo.PropertyType });

    private static readonly MethodInfo createMethodInfo =
      typeof(DefaultPropertyDefinitions).GetMethod(nameof(CreateGeneric), ReflectionConstants.ResolveFlags);

    private PropertyDefinition<T> CreateGeneric<T>(string name) =>
      creators.Aggregate(SingleDefaultProperty<T>.Create(name),
        (property, creator) => new(property, (IPropertyDefinition<T>)creator(typeof(T), name)));
  }
}