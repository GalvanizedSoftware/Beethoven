using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties
{
  public class DefaultPropertyGenerator
  {
    private readonly Func<Type, string, object>[] creators;

    internal DefaultPropertyGenerator(Func<Type, string, object>[] creators)
    {
      this.creators = creators;
    }

    private static readonly MethodInfo createMethodInfo =
      typeof(DefaultPropertyGenerator).GetMethod(nameof(CreateGeneric), ReflectionConstants.ResolveFlags);

    private PropertyDefinition Create(PropertyInfo propertyInfo) =>
      (PropertyDefinition)createMethodInfo.Invoke(this, new object[] { propertyInfo.Name }, new[] { propertyInfo.PropertyType });

    private PropertyDefinition<T> CreateGeneric<T>(string name) =>
      creators.Aggregate(new PropertyDefinition<T>(name),
        (property, creator) => new PropertyDefinition<T>(property, (IPropertyDefinition<T>)creator(typeof(T), name)));

    internal ICodeGenerator GetGenerator(GeneratorContext generatorContext) =>
      Create(generatorContext?.MemberInfo as PropertyInfo)
        .GetGenerator(generatorContext);
  }
}