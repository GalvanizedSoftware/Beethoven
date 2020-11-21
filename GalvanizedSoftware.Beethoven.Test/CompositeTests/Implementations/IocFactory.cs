using System;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Generic.ValueLookup;
using GalvanizedSoftware.Beethoven.Test.CompositeTests.Interfaces;
using Unity;

namespace GalvanizedSoftware.Beethoven.Test.CompositeTests.Implementations
{
  internal class IocFactory
  {
    private readonly BeethovenFactory beethovenFactory;

    public IocFactory(IUnityContainer container)
    {
      IValueLookup lookup = new CompositeValueLookup(new InterfaceFactoryValueLookup((type, name) => container.Resolve(type)));
      beethovenFactory = new BeethovenFactory(
        new DefaultProperty()
          .ValueLookup(lookup)
          .SetterGetter());
      container.RegisterFactory<ICompany>(CreateInstance);
      container.RegisterFactory<ICompanyInformation>(CreateInformationInstance);
    }

    private object CreateInstance(IUnityContainer unityContainer, Type type, string name)
    {
      return beethovenFactory.Generate(type);
    }

    private object CreateInformationInstance(IUnityContainer unityContainer, Type type, string name)
    {
      return beethovenFactory.Generate(type,
        new PropertyDefinition<string>(nameof(ICompanyInformation.Name))
          .Constant(unityContainer.Resolve<string>("informationName")),
        new PropertyDefinition<string>(nameof(ICompanyInformation.Address))
          .Constant(unityContainer.Resolve<string>("informationAddress"))
      );
    }
  }
}
