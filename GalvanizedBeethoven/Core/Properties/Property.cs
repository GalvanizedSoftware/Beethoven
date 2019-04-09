using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  public abstract class Property
  {
    private static readonly Type type = typeof(Property);
    private static readonly MethodInfo createGenericMethodInfo = type.
      GetMethod(nameof(CreateGeneric), Constants.StaticResolveFlags);

    protected Property(string name)
    {
      Name = name;
    }

    protected Property(Property previous) :
      this(previous.Name)
    {
    }

    public string Name { get; }

    public abstract Type PropertyType { get; }

    internal abstract object InvokeGet();

    internal abstract void InvokeSet(object newValue);

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
      else
        return false;
    }
  }
}