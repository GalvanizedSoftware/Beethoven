using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic
{
  public class LinkedObjects : IDefinitions, IMainTypeUser, IBindingParent
  {
    private readonly Dictionary<object, MethodInfo[]> implementationMethods;
    private readonly ExactMethodComparer methodComparer = new ExactMethodComparer();
    private readonly object[] partDefinitions;
    private Type mainType;

    public LinkedObjects(params object[] partDefinitions)
    {
      this.partDefinitions = partDefinitions;
      implementationMethods = partDefinitions
        .ToDictionary(obj => obj, FindMethodInfos);
    }

    private IEnumerable<MethodDefinition> GetMethods() => mainType
        .GetAllMethodsAndInherited()
        .Where(methodInfo => !methodInfo.IsSpecialName)
        .Select(CreateMethod);

    public IEnumerable<PropertyDefinition> GetProperties()
    {
      Dictionary<string, List<PropertyDefinition>> propertiesMap = new Dictionary<string, List<PropertyDefinition>>();
      foreach (PropertyDefinition property in partDefinitions.SelectMany(CreateProperties))
      {
        string propertyName = property.Name;
        if (!propertiesMap.TryGetValue(propertyName, out List<PropertyDefinition> existingProperties))
        {
          existingProperties = new List<PropertyDefinition>();
          propertiesMap.Add(propertyName, existingProperties);
        }
        existingProperties.Add(property);
      }

      foreach (KeyValuePair<string, List<PropertyDefinition>> pair in propertiesMap)
      {
        PropertyDefinition propertyDefinition = PropertyDefinition.Create(pair.Value.First().PropertyType, pair.Key, pair.Value);
        if (propertyDefinition != null)
          yield return propertyDefinition;
      }
    }

    private MethodDefinition CreateMethod(MethodInfo methodInfo)
    {
      if (methodInfo.Name == "Add")
      {
      }
      MethodDefinition[] localMethods = (implementationMethods
        .SelectMany(pair => CreateMethod(pair.Key, pair.Value, methodInfo)))
        .ToArray();
      return methodInfo.HasReturnType() ?
        (MethodDefinition)localMethods.Aggregate(
          new LinkedMethodsReturnValue(methodInfo),
          (value, method) => value.Add(method)) :
        localMethods.Aggregate(
          new LinkedMethods(methodInfo.Name),
          (value, method) => value.Add(method));
    }

    private IEnumerable<PropertyDefinition> CreateProperties(object definition)
    {
      return definition switch
      {
        MethodDefinition _ => Array.Empty<PropertyDefinition>(),
        PropertyDefinition property => new[] { property },
        _ => new PropertiesMapper(mainType, definition),
      };
    }

    private IEnumerable<MethodDefinition> CreateMethod(object definition, MethodInfo[] methodInfos, MethodInfo methodInfo)
    {
      switch (definition)
      {
        case MethodDefinition method:
          if (method.MethodMatcher.IsNonGenericMatch(methodInfo))
            yield return method;
          break;
        default:
          MethodInfo actualMethodInfo = methodInfos
            .FirstOrDefault(item => methodComparer.Equals(methodInfo, item));
          if (actualMethodInfo != null)
            yield return new MappedMethod(actualMethodInfo, definition);
          break;
      }
    }

    private static MethodInfo[] FindMethodInfos(object obj)
    {
      switch (obj)
      {
        case IDefinition _:
          return null;
      }
      return obj?
        .GetType()
        .GetAllTypes()
        .SelectMany(type => type.GetNotSpecialMethods())
        .ToArray();
    }

    public IEnumerable<IDefinition> GetDefinitions()
    {
      foreach (PropertyDefinition property in GetProperties())
        yield return property;
      foreach (MethodDefinition method in GetMethods())
        yield return method;
    }

    public void Set(Type setMainType) =>
      mainType = setMainType;

    public void Bind(object target) => 
      partDefinitions
        .OfType<IBindingParent>()
        .ForEach(bindingParent => bindingParent.Bind(target));
  }
}
