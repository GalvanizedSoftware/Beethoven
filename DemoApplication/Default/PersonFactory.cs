using GalvanizedSoftware.Beethoven.Generic.Properties;

namespace GalvanizedSoftware.Beethoven.DemoApp.Default
{
  internal class PersonFactory
  {
    private readonly BeethovenFactory factory = new BeethovenFactory();

    public IPerson CreatePerson()
    {
      return factory.Generate<IPerson>(
        new DefaultProperty().
          SkipIfEqual().
          SetterGetter().
          NotifyChanged()
      );
    }
  }
}