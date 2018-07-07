using System;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.MVVM.Properties;

namespace GalvanizedSoftware.Beethoven.Extentions
{
  public static class PropertyExtentions
  {
    public static Property<T> ValidityCheck<T>(this Property<T> property, Func<T, bool> checkFunc)
    {
      return new Property<T>(property, new ValidityCheck<T>(checkFunc));
    }

    public static Property<T> RangeCheck<T>(this Property<T> property, T minimum, T maximum) where T : IComparable
    {
      return new Property<T>(property, new RangeCheck<T>(minimum, maximum));
    }

    public static Property<T> SkipIfChanged<T>(this Property<T> property)
    {
      return new Property<T>(property, new SkipIfChanged<T>());
    }

    public static Property<T> SetterGetter<T>(this Property<T> property)
    {
      return new Property<T>(property, new SetterGetter<T>());
    }

    public static Property<T> NotSupported<T>(this Property<T> property)
    {
      return new Property<T>(property, new NotSupported<T>());
    }

    public static Property<T> NotifyChanged<T>(this Property<T> property)
    {
      return new Property<T>(property, new NotifyChanged<T>(property.Name));
    }

    public static Property<T> MappedFrom<T>(this Property<T> property, object main)
    {
      return new Property<T>(property, new Mapped<T>(main, property.Name));
    }

    public static Property<T> MappedGetter<T>(this Property<T> property, Func<T> getterFunc)
    {
      return new Property<T>(property, new DelegatedGetter<T>(getterFunc));
    }

    public static Property<T> Constant<T>(this Property<T> property, T value)
    {
      return new Property<T>(property, new Constant<T>(value));
    }

    public static Property<T> DelegatedSetter<T>(this Property<T> property, Action<T> action)
    {
      return new Property<T>(property, new DelegatedSetter<T>(action));
    }

    public static Property<T> DelegatedGetter<T>(this Property<T> property, Func<T> func)
    {
      return new Property<T>(property, new DelegatedGetter<T>(func));
    }
  }
}