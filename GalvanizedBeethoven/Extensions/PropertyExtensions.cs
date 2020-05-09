using System;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Generic.Parameters;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.MVVM.Properties;

namespace GalvanizedSoftware.Beethoven.Extensions
{
  public static class PropertyExtensions
  {
    public static PropertyDefinition<T> ValidityCheck<T>(this PropertyDefinition<T> propertyDefinition, Func<T, bool> checkFunc) => 
      new PropertyDefinition<T>(propertyDefinition, new ValidityCheck<T>(checkFunc));

    public static PropertyDefinition<T> RangeCheck<T>(this PropertyDefinition<T> propertyDefinition, T minimum, T maximum) where T : IComparable => 
      new PropertyDefinition<T>(propertyDefinition, new RangeCheck<T>(minimum, maximum));

    public static PropertyDefinition<T> SkipIfEqual<T>(this PropertyDefinition<T> propertyDefinition) => 
      new PropertyDefinition<T>(propertyDefinition, new SkipIfEqual<T>());

    public static PropertyDefinition<T> SetterGetter<T>(this PropertyDefinition<T> propertyDefinition) => 
      new PropertyDefinition<T>(propertyDefinition, new SetterGetter<T>());

    public static PropertyDefinition<T> NotSupported<T>(this PropertyDefinition<T> propertyDefinition) => 
      new PropertyDefinition<T>(propertyDefinition, new NotSupported<T>());

    public static PropertyDefinition<T> NotifyChanged<T>(this PropertyDefinition<T> propertyDefinition) => 
      new PropertyDefinition<T>(propertyDefinition, new NotifyChanged<T>(propertyDefinition?.Name));

    public static PropertyDefinition<T> MappedFrom<T>(this PropertyDefinition<T> propertyDefinition, object main) => 
      new PropertyDefinition<T>(propertyDefinition, new Mapped<T>(main, propertyDefinition?.Name));

    public static PropertyDefinition<T> MappedGetter<T>(this PropertyDefinition<T> propertyDefinition, Func<T> getterFunc) => 
      new PropertyDefinition<T>(propertyDefinition, new DelegatedGetter<T>(getterFunc));

    public static PropertyDefinition<T> Constant<T>(this PropertyDefinition<T> propertyDefinition, T value) => 
      new PropertyDefinition<T>(propertyDefinition, new Constant<T>(value));

    public static PropertyDefinition<T> InitialValue<T>(this PropertyDefinition<T> propertyDefinition, T value) => 
      new PropertyDefinition<T>(propertyDefinition, new InitialValue<T>(value));

    public static PropertyDefinition<T> ConstructorParameter<T>(this PropertyDefinition<T> propertyDefinition) => 
      new PropertyDefinition<T>(propertyDefinition, new PropertyConstructorInitialized(propertyDefinition?.Name, typeof(T)));

    public static PropertyDefinition<T> DelegatedSetter<T>(this PropertyDefinition<T> propertyDefinition, Action<T> action) => 
      new PropertyDefinition<T>(propertyDefinition, new DelegatedSetter<T>(action));

    public static PropertyDefinition<T> DelegatedGetter<T>(this PropertyDefinition<T> propertyDefinition, Func<T> func) => 
      new PropertyDefinition<T>(propertyDefinition, new DelegatedGetter<T>(func));

    public static PropertyDefinition<T> LazyCreator<T>(this PropertyDefinition<T> propertyDefinition, Func<T> creatorFunc) => 
      new PropertyDefinition<T>(propertyDefinition, new LazyCreator<T>(creatorFunc));
  }
}