using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Interceptors;
using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  public abstract class Property : IInterceptorProvider
  {
    private static readonly Type type = typeof(Property);
    private static readonly MethodInfo createGenericMethodInfo = type
      .GetMethod(nameof(CreateGeneric), Constants.StaticResolveFlags);

    protected Property(string name, Type propertyType, IParameter parameter = null)
    {
      Name = name;
      PropertyType = propertyType;
      MemberInfo = type.GetProperty(name);
      Parameter = parameter;
    }

    protected Property(Property previous, IParameter parameter = null) :
      this(previous.Name, previous.PropertyType, parameter ?? previous.Parameter)
    {
    }

    public string Name { get; }

    public MemberInfo MemberInfo { get; }

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

    public static Property Create(Type propertyType, string name, IEnumerable<Property> propertyDefinitions)
    {
      return (Property)createGenericMethodInfo
        .MakeGenericMethod(propertyType)
        .Invoke(type, new object[] { name, propertyDefinitions.ToArray() });
    }

    private static Property CreateGeneric<T>(string name, Property[] propertyDefinitions)
    {
      Property<T> property = new Property<T>(name);
      return propertyDefinitions.Length == 0 ?
        property :
        new Property<T>(property, propertyDefinitions.OfType<IPropertyDefinition<T>>().ToArray());
    }

    internal bool IsMatch(MethodInfo methodInfo)
    {
      if (!methodInfo.IsSpecialName)
        return false;
      if (methodInfo.Name == "get_" + Name)
        return methodInfo.ReturnType == PropertyType;
      if (methodInfo.Name == "set_" + Name)
        return methodInfo.GetParameters().SingleOrDefault()?.ParameterType == PropertyType;
      return false;
    }
  }
}