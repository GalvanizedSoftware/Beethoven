using System;
using GalvanizedSoftware.Beethoven.Generic.Properties;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  public static class PropertyExtensions
  {
    public static PropertyDefinition<T> ValidityCheck<T>(this PropertyDefinition<T> propertyDefinition, Func<T, bool> checkFunc) => 
      new(propertyDefinition, new ValidityCheck<T>(checkFunc));

    public static PropertyDefinition<T> RangeCheck<T>(this PropertyDefinition<T> propertyDefinition, T minimum, T maximum) where T : IComparable => 
      new(propertyDefinition, new RangeCheck<T>(minimum, maximum));

    public static PropertyDefinition<T> SkipIfEqual<T>(this PropertyDefinition<T> propertyDefinition) => 
      new(propertyDefinition, new SkipIfEqual<T>());

    public static PropertyDefinition<T> SetterGetter<T>(this PropertyDefinition<T> propertyDefinition) => 
      new(propertyDefinition, new SetterGetter<T>());

    public static PropertyDefinition<T> NotSupported<T>(this PropertyDefinition<T> propertyDefinition) => 
      new(propertyDefinition, new NotSupported<T>());

    public static PropertyDefinition<T> NotifyChanged<T>(this PropertyDefinition<T> propertyDefinition) => 
      new(propertyDefinition, new NotifyChanged<T>(propertyDefinition?.Name));

    public static PropertyDefinition<T> MappedFrom<T>(this PropertyDefinition<T> propertyDefinition, object main) => 
      new(propertyDefinition, new Mapped<T>(main, propertyDefinition?.Name));

    public static PropertyDefinition<T> MappedGetter<T>(this PropertyDefinition<T> propertyDefinition, Func<T> getterFunc) => 
      new(propertyDefinition, new DelegatedGetter<T>(getterFunc));

    public static PropertyDefinition<T> Constant<T>(this PropertyDefinition<T> propertyDefinition, T value) => 
      new(propertyDefinition, new Constant<T>(value));

    public static PropertyDefinition<T> InitialValue<T>(this PropertyDefinition<T> propertyDefinition, T value) => 
      new(propertyDefinition, new InitialValue<T>(value));

    public static PropertyDefinition<T> ConstructorParameter<T>(this PropertyDefinition<T> propertyDefinition) => 
      new(propertyDefinition, new ConstructorInitializedProperty(propertyDefinition?.Name, typeof(T)));

    public static PropertyDefinition<T> DelegatedSetter<T>(this PropertyDefinition<T> propertyDefinition, Action<T> action) => 
      new(propertyDefinition, new DelegatedSetter<T>(action));

    public static PropertyDefinition<T> DelegatedGetter<T>(this PropertyDefinition<T> propertyDefinition, Func<T> func) => 
      new(propertyDefinition, new DelegatedGetter<T>(func));

    public static PropertyDefinition<T> LazyCreator<T>(this PropertyDefinition<T> propertyDefinition, Func<T> creatorFunc) => 
      new(propertyDefinition, new LazyCreator<T>(creatorFunc));
  }
}