using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Binding;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Methods;

namespace GalvanizedSoftware.Beethoven.Generic
{
  public class LinkedObjects : IBindingParent
  {
    private readonly Dictionary<object, MethodInfo[]> implementationMethods;
    private readonly ExactMethodComparer methodComparer = new ExactMethodComparer();
    private readonly object[] partDefinitions;

    public LinkedObjects(params object[] partDefinitions)
    {
      this.partDefinitions = partDefinitions;
      implementationMethods = partDefinitions
        .ToDictionary(obj => obj, FindMethodInfos);
    }

    public IEnumerable GetWrappers<T>() where T : class
    {
      foreach (Property property in GetProperties())
        yield return property;
      foreach (Method method in GetMethods<T>())
        yield return method;
    }

    private IEnumerable<Method> GetMethods<T>() where T : class
    {
      return typeof(T)
        .GetAllMethodsAndInherited()
        .Where(methodInfo => !methodInfo.IsSpecialName)
        .Select(CreateMethod);
    }

    //private IEnumerable<Property> LinkProperties<T>() where T : class
    //{
    //  Property[] properties = partDefinitions.SelectMany(CreateProperties).ToArray();
    //  var linkedProperties = properties
    //    .Select(property => (property.PropertyType, property.Name, property))
    //    .GroupBy(tuple => (tuple.PropertyType, tuple.Name), tuple => tuple)
    //    .ToDictionary(tuples => tuples.Key, tuples => tuples
    //      .Select(tuple => tuple.property)
    //      )
    //    .Select(pair => pair
    //      .Value
    //      .Aggregate(Property.Create(pair.Key.PropertyType, pair.Key.Name),
    //        (existingProperty, property) => existingProperty.Add(property)));

    //  string[] names = properties.Select(property => property.Name).ToArray();
    //  foreach (string name in names)
    //  {
    //    return properties.Where(property => )
    //  }
    //}

    public IEnumerable<Property> GetProperties()
    {
      Dictionary<string, List<Property>> propertiesMap = new Dictionary<string, List<Property>>();
      foreach (Property property in partDefinitions.SelectMany(CreateProperties))
      {
        string propertyName = property.Name;
        if (!propertiesMap.TryGetValue(propertyName, out List<Property> existingProperties))
        {
          existingProperties = new List<Property>();
          propertiesMap.Add(propertyName, existingProperties);
        }
        existingProperties.Add(property);
      }

      foreach (KeyValuePair<string, List<Property>> pair in propertiesMap)
      {
        Property property = Property.Create(pair.Value.First().PropertyType, pair.Key, pair.Value);
        if (property != null)
          yield return property;
      }
    }

    private Method CreateMethod(MethodInfo methodInfo)
    {
      Method[] localMethods = (implementationMethods
        .SelectMany(pair => CreateMethod(pair.Key, pair.Value, methodInfo)))
        .ToArray();
      return methodInfo.HasReturnType() ?
        (Method)localMethods.Aggregate(
          new LinkedMethodsReturnValue(methodInfo.Name),
          (value, method) => value.Add(method)) :
        localMethods.Aggregate(
          new LinkedMethods(methodInfo.Name),
          (value, method) => value.Add(method));
    }

    private static IEnumerable<Property> CreateProperties(object definition)
    {
      switch (definition)
      {
        case Method _:
          return new Property[0];
        case Property property:
          return new[] { property };
        default:
          return new PropertiesMapper(definition);
      }
    }

    private IEnumerable<Method> CreateMethod(object definition, MethodInfo[] methodInfos, MethodInfo methodInfo)
    {
      switch (definition)
      {
        case Method method:
          (Type, string)[] parameterTypes = methodInfo.GetParameterTypeAndNames();
          if (method.IsMatch(parameterTypes, new Type[0], methodInfo.ReturnType))
            yield return method;
          break;
        case Property _:
          yield break;
        default:
          MethodInfo actualMethodInfo = methodInfos
            .FirstOrDefault(item => methodComparer.Equals(methodInfo, item));
          if (actualMethodInfo != null)
            yield return new MappedMethod(definition, actualMethodInfo);
          break;
      }
    }

    private static MethodInfo[] FindMethodInfos(object obj)
    {
      switch (obj)
      {
        case Method _:
        case Property _:
          return null;
      }
      return obj
        .GetType()
        .GetAllTypes()
        .SelectMany(type => type.GetNotSpecialMethods())
        .ToArray();
    }

    public void Bind(object target)
    {
      foreach (IBindingParent bindingParent in partDefinitions.OfType<IBindingParent>())
        bindingParent.Bind(target);
    }
  }
}
