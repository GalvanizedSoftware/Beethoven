using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extentions;

namespace GalvanizedSoftware.Beethoven.DemoApp.DefaultImplementaion
{
  internal class PersonFactory
  {
    private readonly BeethovenFactory factory = new BeethovenFactory();

    public IPerson CreatePerson()
    {
      return factory.Generate<IPerson>(
        new Property<string>("FirstName").
          ValidityCheck(name => !string.IsNullOrEmpty(name)).
          SkipIfEqual().
          SetterGetter().
          NotifyChanged(),
        CreateMyRequiredStringProperty("LastName")
        //new DefaultProperty().
        //  SkipIfEqual().
        //  SetterGetter().
        //  NotifyChanged()
      );
      //XmlEntry(”FirstName”));
    }

    private static Property CreateMyRequiredStringProperty(string propertyName)
    {
      return new Property<string>(propertyName).
        ValidityCheck(name => !string.IsNullOrEmpty(name)).
        SkipIfEqual().
        SetterGetter().
        NotifyChanged();
    }
  }
}