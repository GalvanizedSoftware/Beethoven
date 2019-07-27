using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.ValueLookup;
using GalvanizedSoftware.Beethoven.MVVM.Properties;
using System;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class DefaultProperty
  {
    private readonly Func<Type, string, object>[] creators;
    private static readonly MethodInfo createMethodInfo = 
      typeof(DefaultProperty).GetMethod(nameof(CreateGeneric), Constants.ResolveFlags);

    public DefaultProperty()
    {
      creators = Array.Empty<Func<Type, string, object>>();
    }

    public DefaultProperty(DefaultProperty previous, Func<Type, string, object> creator)
    {
      creators = previous?.creators.Concat(new[] { creator }).ToArray();
    }

    public PropertyDefinition Create(Type type, string name)
    {
      return (PropertyDefinition)createMethodInfo.Invoke(this, new object[] { name }, new[] { type });
    }

    private PropertyDefinition<T> CreateGeneric<T>(string name)
    {
      return creators.Aggregate(new PropertyDefinition<T>(name),
        (property, creator) => new PropertyDefinition<T>(property, (IPropertyDefinition<T>)creator(typeof(T), name)));
    }

    public DefaultProperty ValidityCheck(object target, string methodName)
    {
      return new DefaultProperty(this, (type, name) => typeof(ValidityCheck<>)
        .MakeGenericType(type)
        .InvokeStatic(nameof(ValidityCheck<object>.CreateWithReflection), target, methodName));
    }

    public DefaultProperty SkipIfEqual()
    {
      return new DefaultProperty(this, (type, name) => typeof(SkipIfEqual<>).Create1(type));
    }

    public DefaultProperty SetterGetter()
    {
      return new DefaultProperty(this, (type, name) => typeof(SetterGetter<>).Create1(type));
    }

    public DefaultProperty NotSupported()
    {
      return new DefaultProperty(this, (type, name) => typeof(NotSupported<>).Create1(type));
    }

    public DefaultProperty NotifyChanged()
    {
      return new DefaultProperty(this, (type, name) => typeof(NotifyChanged<>).Create1(type, name));
    }

    public DefaultProperty Constant(Func<Type, object> valueGetter)
    {
      return new DefaultProperty(this, (type, name) => typeof(Constant<>).Create1(type, valueGetter(type)));
    }

    public DefaultProperty DelegatedSetter(object target, string methodName)
    {
      return new DefaultProperty(this, (type, name) => typeof(DelegatedSetter<>)
        .MakeGenericType(type)
        .InvokeStatic(nameof(DelegatedSetter<object>.CreateWithReflection), target, methodName, name));
    }

    public DefaultProperty DelegatedGetter(object target, string methodName)
    {
      return new DefaultProperty(this, (type, name) => typeof(DelegatedGetter<>)
        .MakeGenericType(type)
        .InvokeStatic(nameof(DelegatedGetter<object>.CreateWithReflection), target, methodName, name));
    }

    public DefaultProperty InitialValue(params object[] initialValues)
    {
      return new DefaultProperty(this, (type, name) => typeof(InitialValue<>).Create1(type,
        initialValues.FirstOrDefault(obj => obj?.GetType() == type)));
    }

    public DefaultProperty ValueLookup(IValueLookup valueLookup)
    {
      return new DefaultProperty(this, (type, name) => typeof(InitialValue<>).Create1(type,
        valueLookup.Lookup(type, name).FirstOrDefault()));
    }

    public DefaultProperty AnonymousValueLookup(object defaultValues)
    {
      return new DefaultProperty(this, (type, name) => typeof(InitialValue<>).Create1(type,
        new AnonymousValueLookup(defaultValues).Lookup(type, name).FirstOrDefault()));
    }

    public DefaultProperty LazyCreator<T>(Func<object> creatorFunc)
    {
      return new DefaultProperty(this, (type, name) => Properties.LazyCreator<T>.CreateIfMatch(type, creatorFunc));
    }
  }
}