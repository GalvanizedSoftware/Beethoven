using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Methods;

namespace GalvanizedSoftware.Beethoven.Generic
{
  public class LinkedObjects
  {
    private readonly Tuple<object, MethodInfo>[] implementationMethods;
    private readonly EquivalentMethodComparer methodComparer = new EquivalentMethodComparer();
    public object[] Objects { get; }

    public LinkedObjects(params object[] objects)
    {
      Objects = objects;
      implementationMethods = objects
        .SelectMany(obj => obj
          .GetType()
          .GetAllTypes()
          .SelectMany(type => type.GetNotSpecialMethods())
          .Select(info => new Tuple<object, MethodInfo>(obj, info)))
          .ToArray();
    }

    public IEnumerable<Property> GetProperties()
    {
      IEnumerable<Property> propertiesMappers = Objects
        .Select(obj => new PropertiesMapper(obj))
        .SelectMany(mapper => mapper);
      Dictionary<string, List<Property>> properties = new Dictionary<string, List<Property>>();
      foreach (Property property in propertiesMappers)
      {
        string propertyName = property.Name;
        if (!properties.TryGetValue(propertyName, out List<Property> existingProperties))
        {
          existingProperties = new List<Property>();
          properties.Add(propertyName, existingProperties);
        }
        existingProperties.Add(property);
      }
      return properties.Select(pair =>
        Property.Create(pair.Value.First().PropertyType, pair.Key, pair.Value));
    }

    public IEnumerable<Method> GetMethods<T>() where T : class =>
      typeof(T).GetAllMethodsAndInherited().Select(CreateMethod);

    private Method CreateMethod(MethodInfo methodInfo)
    {
      Tuple<object, MethodInfo>[] localMethods = implementationMethods
        .Where(tuple => methodComparer.Equals(methodInfo, tuple.Item2))
        .ToArray();
      return methodInfo.HasReturnType() ?
        (Method)localMethods.Aggregate(
          new LinkedMethodsReturnValue(methodInfo.Name),
          (value, tuple) => value.MappedMethod(tuple.Item1, tuple.Item2)) :
        localMethods.Aggregate(
          new LinkedMethods(methodInfo.Name),
          (value, tuple) => value.MappedMethod(tuple.Item1, tuple.Item2));
    }
  }
}
