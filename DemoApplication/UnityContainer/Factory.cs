using GalvanizedSoftware.Beethoven.Generic.Properties;
using System;
using Unity;
using Unity.Injection;

namespace GalvanizedSoftware.Beethoven.DemoApp.UnityContainer
{
  internal class Factory
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();

    public Factory(IUnityContainer container)
    {
      container.RegisterFactory<IPerson>(CreateMvvmInstance);
      container.RegisterFactory<IUnityViewModel>(CreateRegularInstance);
    }

    private object CreateMvvmInstance(IUnityContainer unityContainer, Type type, string name)
    {
      return beethovenFactory.Generate(type,
        new DefaultProperty().
          SkipIfEqual().
          SetterGetter().
          NotifyChanged()
      );
    }

    private object CreateRegularInstance(IUnityContainer unityContainer, Type type, string name)
    {
      return beethovenFactory.Generate(type,
        new DefaultProperty()
          .LazyCreator<IPerson>(() => unityContainer.Resolve<IPerson>())
          .SetterGetter()
      );
    }
  }
}
