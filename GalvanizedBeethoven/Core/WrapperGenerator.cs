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

    private WrapperGenerator(object[] partDefinitions,
      Func<object, IEnumerable<object>> getWrappers)
    {
      this.partDefinitions = partDefinitions;
      this.getWrappers = getWrappers;
    }

    private WrapperGenerator(WrapperGenerator<T> baseWrapperGenerator,
      IEnumerable<object> additionsObjects)
    {
      partDefinitions = baseWrapperGenerator.partDefinitions
        .Concat(additionsObjects)
        .ToArray();
      getWrappers = baseWrapperGenerator.getWrappers;
    }

    internal static List<object> GetWrappers(object[] partDefinitions)
    {
      return new WrapperGenerator<T>(partDefinitions, GetDefinitionWrappers)
        .AddDefaultImplementationWrappers()
        .ToList();
    }

    public IEnumerator<object> GetEnumerator()
    {
      foreach (object definition in partDefinitions)
      {
        switch (definition)
        {
          case null:
            break;
          case Property property:
            yield return property;
            break;
          case IEnumerable<Property> properties:
            foreach (Property property in properties)
              yield return property;
            break;
          case Method method:
            yield return method;
            break;
          case IEnumerable<Method> methods:
            foreach (Method method in methods)
              yield return method;
            break;
          case DefaultProperty _:
          case DefaultMethod _:
            // Dependent on what other wrappers are in there, so it has to be evaluated last
            break;
          case LinkedObjects linkedObjects:
            foreach (object wrapper in linkedObjects.GetWrappers<T>())
              yield return wrapper;
            break;
          case IParameter parameter:
            yield return parameter;
            break;
          case DefinitionImport definitionImport:
            foreach (object wrapper in getWrappers(definitionImport))
              yield return wrapper;
            break;
          case IEnumerable<DefinitionImport> imports:
            foreach (DefinitionImport definitionImport in imports.SelectMany(definitionImport => getWrappers(definitionImport)))
              yield return getWrappers(definitionImport);
            break;
          default:
            foreach (object wrapper in getWrappers(definition))
              yield return wrapper;
            break;
        }
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private WrapperGenerator<T> AddDefaultImplementationWrappers() =>
      new WrapperGenerator<T>(this, GetDefaultProperties(partDefinitions, this.OfType<Property>())
        .Concat(GetDefaultMethods(partDefinitions)));

    private static IEnumerable<object> GetDefaultProperties(IEnumerable<object> partDefinitions,
    IEnumerable<Property> propertyWrappers)
    {
      DefaultProperty[] defaultProperties = partDefinitions.OfType<DefaultProperty>().ToArray();
      if (!defaultProperties.Any())
        yield break;
      DefaultProperty defaultProperty = defaultProperties.Single();
      IDictionary<string, Type> propertyInfos = typeof(T)
        .GetProperties(ResolveFlags)
        .ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.PropertyType);
      string[] typeProperties = propertyInfos.Keys.ToArray();
      HashSet<string> alreadyImplemented = new HashSet<string>(propertyWrappers.Select(p => p.Name));
      foreach (string propertyName in typeProperties.Except(alreadyImplemented))
        yield return defaultProperty.Create(propertyInfos[propertyName], propertyName);
    }

    private static IEnumerable<object> GetDefaultMethods(IEnumerable<object> partDefinitions)
    {
      DefaultMethod defaultMethod = partDefinitions.OfType<DefaultMethod>().SingleOrDefault();
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
