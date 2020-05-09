using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.ValueLookup;
using GalvanizedSoftware.Beethoven.MVVM.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class DefaultProperty : ICodeGenerator, IDefinition
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

    public PropertyDefinition Create(Type type, string name) =>
      (PropertyDefinition)createMethodInfo.Invoke(this, new object[] { name }, new[] { type });

    private PropertyDefinition<T> CreateGeneric<T>(string name) =>
      creators.Aggregate(new PropertyDefinition<T>(name),
        (property, creator) => new PropertyDefinition<T>(property, (IPropertyDefinition<T>)creator(typeof(T), name)));

    public DefaultProperty ValidityCheck(object target, string methodName) =>
      new DefaultProperty(this,
        (type, name) => ValidityCheckFactory.Create(type, target, methodName));

    public DefaultProperty SkipIfEqual() =>
      new DefaultProperty(this, (type, name) => typeof(SkipIfEqual<>).Create1(type));

    public DefaultProperty SetterGetter() =>
      new DefaultProperty(this, (type, name) => typeof(SetterGetter<>).Create1(type));

    public DefaultProperty NotSupported() =>
      new DefaultProperty(this, (type, name) => typeof(NotSupported<>).Create1(type));

    public DefaultProperty NotifyChanged() =>
      new DefaultProperty(this, (type, name) => typeof(NotifyChanged<>).Create1(type, name));

    public DefaultProperty Constant(Func<Type, object> valueGetter) =>
      new DefaultProperty(this, (type, name) => typeof(Constant<>).Create1(type, valueGetter(type)));

    public DefaultProperty DelegatedSetter(object target, string methodName) =>
      new DefaultProperty(this,
        (type, name) => DelegatedSetterFactory.Create(type, target, methodName, name));

    public DefaultProperty DelegatedGetter(object target, string methodName) =>
      new DefaultProperty(this, (type, name) =>
        DelegatedGetterFactory.Create(type, target, methodName, name));

    public DefaultProperty InitialValue(params object[] initialValues) =>
      new DefaultProperty(this, (type, name) => typeof(InitialValue<>).Create1(type,
        initialValues.FirstOrDefault(obj => obj?.GetType() == type)));

    public DefaultProperty ValueLookup(IValueLookup valueLookup) =>
      new DefaultProperty(this, (type, name) => typeof(InitialValue<>).Create1(type,
        valueLookup.Lookup(type, name).FirstOrDefault()));

    public DefaultProperty AnonymousValueLookup(object defaultValues) =>
      new DefaultProperty(this, (type, name) => typeof(InitialValue<>).Create1(type,
        new AnonymousValueLookup(defaultValues).Lookup(type, name).FirstOrDefault()));

    public DefaultProperty LazyCreator<T>(Func<object> creatorFunc) =>
      new DefaultProperty(this, (type, name) => LazyCreatorFactory.CreateIfMatch<T>(type, creatorFunc));

    public int SortOrder => 2;

    public bool CanGenerate(MemberInfo memberInfo) =>
      memberInfo switch
      {
        PropertyInfo _ => true,
        _ => false,
      };

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
      PropertyInfo propertyInfo = generatorContext?.MemberInfo as PropertyInfo;
      return propertyInfo == null ?
        Enumerable.Empty<string>() :
        Create(propertyInfo.PropertyType, propertyInfo.Name)
        .GetGenerator()
        .Generate(generatorContext);
    }

    public ICodeGenerator GetGenerator() =>
      this;
  }
}