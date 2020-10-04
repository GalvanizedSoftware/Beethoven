using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Events;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using System.ComponentModel;

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
        CreateMvvmStringProperty("LastName"),
        new SimpleEventDefinition<PropertyChangedEventHandler>(nameof(INotifyPropertyChanged.PropertyChanged))
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