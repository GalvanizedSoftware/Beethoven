using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extentions;
using GalvanizedSoftware.Beethoven.MVVM.Properties;
using System;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class DefaultProperty
  {
    private readonly Func<Type, string, object>[] creators;
    private static readonly MethodInfo createMethodInfo;

    static DefaultProperty()
    {
      createMethodInfo = typeof(DefaultProperty).GetMethod(nameof(CreateGeneric), Constants.ResolveFlags);
    }

    public DefaultProperty()
    {
      creators = new Func<Type, string, object>[0];
    }

    public DefaultProperty(DefaultProperty previous, Func<Type, string, object> creator)
    {
      creators = previous.creators.Concat(new[] { creator }).ToArray();
    }

    public Property Create(Type type, string name)
    {
      return (Property)createMethodInfo.Invoke(this, new object[] { name }, new[] { type });
    }

    private Property<T> CreateGeneric<T>(string name)
    {
      return creators.Aggregate(new Property<T>(name),
        (property, creator) => new Property<T>(property, (IPropertyDefinition<T>)creator(typeof(T), name)));
    }

    public DefaultProperty ValidityCheck(object target, string methodName)
    {
      return new DefaultProperty(this, (type, name) =>
      {
        MethodInfo methodInfo = target.GetType().GetMethod(methodName, Constants.ResolveFlags);
        return typeof(ValidityCheck<>)
          .MakeGenericType(type)
          .InvokeStatic(nameof(ValidityCheck<object>.CreateWithReflection), target, methodName);
      });
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
      return new DefaultProperty(this, (type, name) =>
      {
        MethodInfo methodInfo = target.GetType().GetMethod(methodName, Constants.ResolveFlags);
        return typeof(DelegatedSetter<>)
          .MakeGenericType(type)
          .InvokeStatic(nameof(DelegatedSetter<object>.CreateWithReflection), target, methodName, name);
      });
    }
  }
}