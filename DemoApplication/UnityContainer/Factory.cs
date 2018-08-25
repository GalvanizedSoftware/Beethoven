using GalvanizedSoftware.Beethoven.Generic.Properties;
using Unity;
using Unity.Injection;

namespace GalvanizedSoftware.Beethoven.DemoApp.UnityContainer
{
  internal class Factory
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();

    public Factory(IUnityContainer container)
    {
      container.RegisterType<IPerson>(new InjectionFactory(_ => Create<IPerson>()));
    }

    private T Create<T>() where T : class
    {
      return beethovenFactory.Generate<T>(
        new DefaultProperty().
          SkipIfEqual().
          SetterGetter().
          NotifyChanged()
      );
    }
  }
}
