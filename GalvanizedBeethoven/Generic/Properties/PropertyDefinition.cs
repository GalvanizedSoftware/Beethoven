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
  public abstract class PropertyDefinition : IDefinition, IDefinitions
  {
    private static readonly Type type = typeof(PropertyDefinition);
    private static readonly MethodInfo createGenericMethodInfo = type
      .GetMethod(nameof(CreateGeneric), ReflectionConstants.StaticResolveFlags);
    private readonly IDefinition[] additionalDefinitions;

    protected PropertyDefinition(string name, Type propertyType, params IDefinition[] additionalDefinitions)
    {
      Name = name;
      PropertyType = propertyType;
      this.additionalDefinitions = additionalDefinitions;
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
    internal abstract object[] Definitions { get; }

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

    public bool CanGenerate(MemberInfo memberInfo) =>
      memberInfo.Name == Name && (memberInfo as PropertyInfo)?.PropertyType == PropertyType;

    public ICodeGenerator GetGenerator(GeneratorContext generatorContext) =>
      new PropertyGenerator(generatorContext, generatorContext?.MemberInfo as PropertyInfo, this);

    public IEnumerable<IDefinition> GetDefinitions()
    {
      foreach (IDefinition definition in additionalDefinitions)
        yield return definition;
      yield return this;
    }
  }
}