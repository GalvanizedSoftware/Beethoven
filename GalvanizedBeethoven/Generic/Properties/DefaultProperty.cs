using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.ValueLookup;
using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Definitions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class DefaultProperty : IDefinitions
  {
    private readonly Func<Type, string, object>[] creators;
    private readonly DefaultPropertyDefinitions propertyDefinitions;

    public DefaultProperty()
    {
      creators = Array.Empty<Func<Type, string, object>>();
      propertyDefinitions = new(creators);
    }

    public DefaultProperty(DefaultProperty previous, Func<Type, string, object> creator)
    {
      creators = previous?.creators.Concat(new[] { creator }).ToArray();
      propertyDefinitions = new(creators);
    }

    public DefaultProperty ValidityCheck(object target, string methodName) =>
      new(this,
        (type, name) => ValidityCheckFactory.Create(type, target, methodName));

    public DefaultProperty SkipIfEqual() =>
      new(this, (type, name) => typeof(SkipIfEqual<>).Create1(type));

    public DefaultProperty SetterGetter() =>
      new(this, (type, name) => typeof(SetterGetter<>).Create1(type));

    public DefaultProperty NotSupported() =>
      new(this, (type, name) => typeof(NotSupported<>).Create1(type));

    public DefaultProperty NotifyChanged() =>
      new(this, (type, name) => typeof(NotifyChanged<>).Create1(type, name));

    public DefaultProperty Constant(Func<Type, object> valueGetter) =>
      new(this, (type, name) => typeof(Constant<>).Create1(type, valueGetter(type)));

    public DefaultProperty DelegatedSetter(object target, string methodName) =>
      new(this,
        (type, name) => DelegatedSetterFactory.Create(type, target, methodName, name));

    public DefaultProperty DelegatedGetter(object target, string methodName) =>
      new(this, (type, name) =>
        DelegatedGetterFactory.Create(type, target, methodName, name));

    public DefaultProperty InitialValue(params object[] initialValues) =>
      new(this, (type, name) => typeof(InitialValue<>).Create1(type,
        initialValues.FirstOrDefault(obj => obj?.GetType() == type)));

    public DefaultProperty ValueLookup(IValueLookup valueLookup) =>
      new(this, (type, name) => typeof(InitialValue<>).Create1(type,
        valueLookup.Lookup(type, name).FirstOrDefault()));

    public DefaultProperty AnonymousValueLookup(object defaultValues) =>
      new(this, (type, name) => typeof(InitialValue<>).Create1(type,
        new AnonymousValueLookup(defaultValues).Lookup(type, name).FirstOrDefault()));

    public DefaultProperty LazyCreator<T>(Func<object> creatorFunc) =>
      new(this, (type, name) => LazyCreatorFactory.CreateIfMatch<T>(type, creatorFunc));

    //public override ICodeGenerator GetGenerator(GeneratorContext generatorContext) =>
    //  new DefaultPropertyGenerator(new(creators)).GetGenerator(generatorContext);

    public IEnumerable<IDefinition> GetDefinitions<TInterface>() where TInterface : class
    {
      MemberInfoList memberInfoList = MemberInfoListCache.Get<TInterface>();
      return memberInfoList
        .PropertyInfos
        .Select(propertyInfo => propertyDefinitions.Create(propertyInfo));
    }
  }
}
