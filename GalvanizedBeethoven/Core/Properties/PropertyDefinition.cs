using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Core.Properties
{
  public abstract class PropertyDefinition : IDefinition, IEnumerable<IDefinition>
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
    internal abstract object[] Definitions { get; }

    public IParameter Parameter { get; }

    public int SortOrder => 1;

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

    public bool CanGenerate(MemberInfo memberInfo) =>
      memberInfo switch
      {
        PropertyInfo propertyInfo =>
            propertyInfo.Name == Name && propertyInfo.PropertyType == PropertyType,
        _ => false,
      };

    public ICodeGenerator GetGenerator() =>
      new PropertyGenerator(this);

    public IEnumerator<IDefinition> GetEnumerator()
    {
      if (Parameter is IDefinition definition)
        yield return definition;
      yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}