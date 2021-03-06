﻿using System;
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
  public class LinkedObjects : IBindingParent, IImports
  {
    private readonly Dictionary<object, MethodInfo[]> implementationMethods;
    private readonly ExactMethodComparer methodComparer = new();
    private readonly object[] partDefinitions;

    public LinkedObjects(params object[] partDefinitions)
    {
      this.partDefinitions = partDefinitions;
      implementationMethods = partDefinitions
        .ToDictionary(obj => obj, FindMethodInfos);
    }

    private IEnumerable<MethodDefinition> GetMethods<T>() => typeof(T)
        .GetAllMethodsAndInherited()
        .Where(methodInfo => !methodInfo.IsSpecialName)
        .Select(CreateMethod);

    public IEnumerable<PropertyDefinition> GetProperties<T>()
    {
      Dictionary<string, List<PropertyDefinition>> propertiesMap = new();
      foreach (PropertyDefinition property in partDefinitions.SelectMany(CreateProperties<T>))
      {
        string propertyName = property.Name;
        if (!propertiesMap.TryGetValue(propertyName, out List<PropertyDefinition> existingProperties))
        {
          existingProperties = new List<PropertyDefinition>();
          propertiesMap.Add(propertyName, existingProperties);
        }
        existingProperties.Add(property);
      }
      return  propertiesMap
        .Select(pair => 
          PropertyDefinition.Create(pair.Value.First().PropertyType, pair.Key, pair.Value))
        .Where(propertyDefinition => propertyDefinition != null);
    }

    private MethodDefinition CreateMethod(MethodInfo methodInfo)
    {
      MethodDefinition[] localMethods = implementationMethods
        .Select(pair => CreateMethod(pair.Key, pair.Value, methodInfo))
        .SkipNull()
        .ToArray();
      return methodInfo.HasReturnType() ?
        (MethodDefinition)localMethods.Aggregate(
          new LinkedMethodsReturnValue(methodInfo),
          (value, method) => value.Add(method)) :
        localMethods.Aggregate(
          new LinkedMethods(methodInfo.Name),
          (value, method) => value.Add(method));
    }

    private static IEnumerable<PropertyDefinition> CreateProperties<T>(object definition) =>
      definition switch
      {
        MethodDefinition => Array.Empty<PropertyDefinition>(),
        PropertyDefinition property => new[] { property },
        _ => new PropertiesMapper(typeof(T), definition),
      };

    private MethodDefinition CreateMethod(object definition, MethodInfo[] methodInfos, MethodInfo methodInfo) =>
      definition switch
      {
        MethodDefinition method => 
          method.MethodMatcher.IsNonGenericMatch(methodInfo) ? method : null,
        _ => 
          MappedMethod.Create(
            methodInfos.FirstOrDefault(item => methodComparer.Equals(methodInfo, item)), 
            definition)
      };

    private static MethodInfo[] FindMethodInfos(object obj) =>
      obj is IDefinition ? null :
      obj?.GetType().GetAllTypes().SelectMany(type => type.GetNotSpecialMethods()).ToArray();

    public IEnumerable<IDefinition> GetDefinitions<T>() where T : class
    {
			foreach (PropertyDefinition property in GetProperties<T>())
				yield return property;
			foreach (MethodDefinition method in GetMethods<T>())
				yield return method;
		}

    public void Bind(object target) => 
      partDefinitions
        .OfType<IBindingParent>()
        .ForEach(bindingParent => bindingParent.Bind(target));

    public IEnumerable<object> GetImports()=> 
	    partDefinitions;
  }
}
