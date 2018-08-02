using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extentions;

namespace GalvanizedSoftware.Beethoven.DemoApp.Mapped
{
  internal class PersonFactory
  {
    private readonly BeethovenFactory factory = new BeethovenFactory();

    public IPerson CreatePerson(IAddress address)
    {
      FullName fullName = new FullName();
      return factory.Generate<IPerson>(
        // All properties are automatically mapped
        fullName, 

        // Manual mapping
        new Property<string>(nameof(IPerson.FullAddress)).MappedGetter(() => address.FullAddress),
        new Property<string>(nameof(IPerson.Country)).Constant(address.Country)
      );
    }
  }
}