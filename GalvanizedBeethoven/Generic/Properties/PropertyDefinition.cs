﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public abstract class PropertyDefinition : DefaultDefinition, IDefinitions, IFieldMaps
  {
    private static readonly Type type = typeof(PropertyDefinition);
    private static readonly MethodInfo createGenericMethodInfo = type
      .GetMethod(nameof(CreateGeneric), ReflectionConstants.StaticResolveFlags);
    private readonly IDefinition[] additionalDefinitions;
    private readonly string invokerName;
    private protected Func<object> invokerFactory;

    protected PropertyDefinition(string name, Type propertyType, params IDefinition[] additionalDefinitions)
    {
      Name = name;
      PropertyType = propertyType;
      this.additionalDefinitions = additionalDefinitions;
      invokerName = $"invoker{name}";
    }

    protected PropertyDefinition(PropertyDefinition previous, params IDefinition[] additionalDefinitions) :
      this(previous?.Name, previous?.PropertyType)
    {
      this.additionalDefinitions = previous?.additionalDefinitions?
        .Concat(additionalDefinitions)
        .ToArray() ?? throw new NullReferenceException();
    }

    public string Name { get; }

    public Type PropertyType { get; }

    public override bool CanGenerate(MemberInfo memberInfo) =>
      memberInfo?.Name == Name && (memberInfo as PropertyInfo)?.PropertyType == PropertyType;

    public IEnumerable<(string, object)> GetFields()
    {
      yield return (invokerName, invokerFactory());
    }

    public static PropertyDefinition Create(Type propertyType, string name, IEnumerable<PropertyDefinition> propertyDefinitions)
    {
      return (PropertyDefinition)createGenericMethodInfo
        .MakeGenericMethod(propertyType)
        .Invoke(type, new object[] { name, propertyDefinitions.ToArray() });
    }

    private static PropertyDefinition CreateGeneric<T>(string name, PropertyDefinition[] propertyDefinitions)
    {
      PropertyDefinition<T> propertyDefinition = new(name);
      return propertyDefinitions.Length == 0 ?
        propertyDefinition :
        new PropertyDefinition<T>(propertyDefinition, propertyDefinitions.OfType<IPropertyDefinition<T>>().ToArray());
    }

    public override ICodeGenerator GetGenerator(MemberInfo memberInfo) =>
      new PropertyGenerator(memberInfo as PropertyInfo);

    public IEnumerable<IDefinition> GetDefinitions<T>() where T : class
    {
      foreach (IDefinition definition in additionalDefinitions)
        yield return definition;
      yield return this;
    }
  }
}
