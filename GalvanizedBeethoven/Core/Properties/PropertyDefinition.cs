using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Interceptors;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  public abstract class PropertyDefinition : IInterceptorProvider
  {
    private static readonly Type type = typeof(PropertyDefinition);
    private static readonly MethodInfo createGenericMethodInfo = type
      .GetMethod(nameof(CreateGeneric), Constants.StaticResolveFlags);

    protected PropertyDefinition(string name, Type propertyType, IParameter parameter = null)
    {
      Name = name;
      PropertyType = propertyType;
      Parameter = parameter;
    }

    protected PropertyDefinition(PropertyDefinition previous, IParameter parameter = null) :
      this(previous?.Name, previous?.PropertyType, parameter ?? previous?.Parameter)
    {
    }

    public string Name { get; }

    public Type PropertyType { get; }
    public IParameter Parameter { get; }

    public IEnumerable<InterceptorMap> GetInterceptorMaps<T>()
    {
      PropertyInfo propertyInfo = typeof(T).GetProperty(Name);
      if (propertyInfo == null)
        yield break;
      yield return new InterceptorMap(propertyInfo.GetMethod, new PropertyGetInterceptor(this));
      yield return new InterceptorMap(propertyInfo.SetMethod, new PropertySetInterceptor(this));
    }

    internal abstract object InvokeGet(InstanceMap instanceMap);

    internal abstract void InvokeSet(InstanceMap instanceMap, object newValue);

    public static PropertyDefinition Create(Type propertyType, string name, IEnumerable<PropertyDefinition> propertyDefinitions)
    {
      return (PropertyDefinition)createGenericMethodInfo
        .MakeGenericMethod(propertyType)
        .Invoke(type, new object[] { name, propertyDefinitions.ToArray() });
    }

    private static PropertyDefinition CreateGeneric<T>(string name, PropertyDefinition[] propertyDefinitions)
    {
      PropertyDefinition<T> propertyDefinition = new PropertyDefinition<T>(name);
      return propertyDefinitions.Length == 0 ?
        propertyDefinition :
        new PropertyDefinition<T>(propertyDefinition, propertyDefinitions.OfType<IPropertyDefinition<T>>().ToArray());
    }

    internal bool IsMatch(MethodInfo methodInfo)
    {
      string name = methodInfo.Name;
      return methodInfo.IsSpecialName &&
              (name == "get_" + Name ? methodInfo.ReturnType == PropertyType :
               name == "set_" + Name &&
               methodInfo.GetParametersSafe().SingleOrDefault()?.ParameterType == PropertyType);
    }
  }
}