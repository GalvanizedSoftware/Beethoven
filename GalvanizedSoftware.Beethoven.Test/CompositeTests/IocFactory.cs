using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Generic.ValueLookup;
using System;
using Unity;
using Unity.Injection;

namespace GalvanizedSoftware.Beethoven.Test.CompositeTests
{
  internal class IocFactory
  {
    private readonly BeethovenFactory beethovenFactory;

    public IocFactory(IUnityContainer container)
    {
      IValueLookup lookup = new CompositeValueLookup(new InterfaceFactoryValueLookup((type, name) => container.Resolve(type)));
      beethovenFactory = new BeethovenFactory
      {
        GeneralPartDefinitions = new object[]
        {
          new DefaultProperty()
            .ValueLookup(lookup)
            .SetterGetter()
        }
      };
      container.RegisterType<ICompany>(new InjectionFactory(CreateInstance));
      container.RegisterType<ICompanyInformation>(new InjectionFactory(CreateInformationInstance));
    }

    private object CreateInstance(IUnityContainer unityContainer, Type type, string name)
    {
      return beethovenFactory.Generate(type);
    }

    private object CreateInformationInstance(IUnityContainer unityContainer, Type type, string name)
    {
      return beethovenFactory.Generate(type,
        new Property<string>(nameof(ICompanyInformation.Name))
          .Constant(unityContainer.Resolve<string>("informationName")),
        new Property<string>(nameof(ICompanyInformation.Address))
          .Constant(unityContainer.Resolve<string>("informationAddress"))
      );
    }
  }
}
