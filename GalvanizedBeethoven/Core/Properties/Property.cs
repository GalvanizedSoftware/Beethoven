using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  public abstract class Property
  {
    private static readonly Type type = typeof(Property);

    public Property(string name)
    {
      Name = name;
    }

    public Property(Property previous) :
      this(previous.Name)
    {
    }

    public string Name { get; }

    public abstract Type PropertyType { get; }

    internal abstract object InvokeGet();

    internal abstract void InvokeSet(object newValue);

    public static Property Create(Type propertyType, string name, params object[] propertyDefinitions)
    {
      return (Property)type.
        GetMethod(nameof(CreateGeneric), BindingFlags.Static)?.
        MakeGenericMethod(propertyType).
        Invoke(type, new object[] { name, propertyDefinitions });
    }

    public static Property CreateGeneric<T>(string name, IPropertyDefinition<T>[] propertyDefinitions)
    {
      Property<T> property = new Property<T>(name);
      return propertyDefinitions.Length == 0 ?
        property :
        new Property<T>(property, propertyDefinitions);
    }
  }
}