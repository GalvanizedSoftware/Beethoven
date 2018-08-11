using GalvanizedSoftware.Beethoven.Core.Properties;
using System.Collections.ObjectModel;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using System.Globalization;
using GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdate.InterfacesV2;
using GalvanizedSoftware.Beethoven.Extentions;

namespace GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdate.Server
{
  internal class ServerModel
  {
    private readonly BeethovenFactory factory = new BeethovenFactory();
    private readonly CultureInfo cultureInfo = new CultureInfo("da-dk", false); // Culture used before on client and server

    public ObservableCollection<IPerson> People { get; } = new ObservableCollection<IPerson>();

    public T CreatePerson<T>() where T : class
    {
      IPerson person = factory.Generate<IPerson>(
        new DefaultProperty()
        .SkipIfEqual()
        .SetterGetter()
        .NotifyChanged());
      if (person is T returnValue)
      {
        People.Add(person);
        return returnValue;
      }
      // A better solution would be to define the interfaces in different assemblies with different version number
      if (typeof(T).FullName == "GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdate.InterfacesV1.IPerson")
      {
        People.Add(person);
        PersonV2ToV1Converter converter = new PersonV2ToV1Converter(person, cultureInfo);
        return factory.Generate<T>(
          person,
          new Property<string>("BirthDate")
          .DelegatedGetter(converter.GetBirthDateString)
          .DelegatedSetter(converter.SetBirthDateDateTime),
          new Property<string>("Country")
          .Constant(cultureInfo.Name)
        );
      }
      return default(T); // Future version, code is not forward compatible, sorry!
    }
  }
}
