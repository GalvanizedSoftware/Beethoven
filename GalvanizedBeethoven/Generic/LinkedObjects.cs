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
    private readonly Dictionary<object, MethodInfo[]> implementationMethods;
    private readonly ExactMethodComparer methodComparer = new ExactMethodComparer();
    public object[] Objects { get; }

    public LinkedObjects(params object[] objects)
    {
      Objects = objects;
      implementationMethods = objects
        .ToDictionary(obj => obj,
          obj => obj
          .GetType()
          .GetAllTypes()
          .SelectMany(type => type.GetNotSpecialMethods())
          .ToArray());
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
      (object, MethodInfo)[] localMethods = (
        from pair in implementationMethods
        let method = pair.Value.FirstOrDefault(item => methodComparer.Equals(methodInfo, item))
        where method != null
        select (pair.Key, method))
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
