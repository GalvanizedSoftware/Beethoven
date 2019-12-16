using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.DemoApp.Basic
{
  internal class PersonFactory
  {
    private readonly BeethovenFactory factory = new BeethovenFactory();

    public IPerson CreatePerson()
    {
      return factory.Generate<IPerson>(
        new PropertyDefinition<string>("FirstName").
          ValidityCheck(name => !string.IsNullOrEmpty(name)).
          SkipIfEqual().
          SetterGetter().
          NotifyChanged(),
        CreateMvvmStringProperty("LastName")
      );
    }

    private static PropertyDefinition CreateMvvmStringProperty(string propertyName)
    {
      return new PropertyDefinition<string>(propertyName).
        ValidityCheck(name => !string.IsNullOrEmpty(name)).
        SkipIfEqual().
        SetterGetter().
        NotifyChanged();
    }
  }
}