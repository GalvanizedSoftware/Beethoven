using System;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Generic.Parameters;
using static GalvanizedSoftware.Beethoven.Core.Constants;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class WrapperGenerator<T> : IEnumerable<object> where T : class
  {
    private readonly object[] partDefinitions;
    private readonly Func<object, IEnumerable<object>> getWrappers;
    private readonly DefaultProperty defaultProperty;
    private readonly DefaultMethod defaultMethod;

    private WrapperGenerator(object[] partDefinitions, Func<object, IEnumerable<object>> getWrappers)
    {
      object[] flatDefinitions = Flatten(partDefinitions).ToArray();
      this.partDefinitions = flatDefinitions;
      this.getWrappers = getWrappers;
      defaultProperty = partDefinitions.OfType<DefaultProperty>().SingleOrDefault();
      defaultMethod = partDefinitions.OfType<DefaultMethod>().SingleOrDefault();
      this.partDefinitions = this.partDefinitions
        .Concat(GetDefaultProperties(flatDefinitions.OfType<Property>()))
        .Concat(GetDefaultMethods())
        .ToArray();
    }

    internal static IEnumerable<object> GetWrappers(object[] partDefinitions) =>
      new WrapperGenerator<T>(partDefinitions, GetDefinitionWrappers);

    public IEnumerator<object> GetEnumerator()
    {
      foreach (object definition in partDefinitions)
      {
        switch (definition)
        {
          case IParameter _:
          case Property _:
          case Method _:
            yield return definition;
            break;
          case LinkedObjects linkedObjects:
            foreach (object wrapper in linkedObjects.GetWrappers<T>())
              yield return wrapper;
            break;
          default:
            foreach (object wrapper in getWrappers(definition))
              yield return wrapper;
            break;
        }
      }
    }

    private static IEnumerable<object> Flatten(IEnumerable<object> objects)
    {
      foreach (object definition in objects)
      {
        switch (definition)
        {
          case null:
            break;
          case IEnumerable<Property> properties:
            foreach (Property property in properties)
              yield return property;
            break;
          case IEnumerable<Method> methods:
            foreach (Method method in methods)
              yield return method;
            break;
          case IEnumerable<DefinitionImport> imports:
            foreach (DefinitionImport definitionImport in imports)
              yield return definitionImport;
            break;
          default:
            yield return definition;
            break;
        }
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private IEnumerable<object> GetDefaultProperties(IEnumerable<Property> propertyWrappers)
    {
      if (defaultProperty == null)
        yield break;
      IDictionary<string, Type> propertyInfos = typeof(T)
        .GetProperties(ResolveFlags)
        .ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.PropertyType);
      string[] typeProperties = propertyInfos.Keys.ToArray();
      HashSet<string> alreadyImplemented = new HashSet<string>(propertyWrappers.Select(p => p.Name));
      foreach (string propertyName in typeProperties.Except(alreadyImplemented))
        yield return defaultProperty.Create(propertyInfos[propertyName], propertyName);
    }

    private IEnumerable<object> GetDefaultMethods()
    {
      if (defaultMethod == null)
        yield break;
      foreach (MethodInfo methodInfo in typeof(T).GetMethodsAndInherited())
        yield return defaultMethod.CreateMapped(methodInfo);
    }

    private static IEnumerable<object> GetDefinitionWrappers(object definition) =>
      new object[0]
        .Concat(new PropertiesMapper(definition))
        .Concat(new MethodsMapper<T>(definition));

  }
}
