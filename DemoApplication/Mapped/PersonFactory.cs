using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic.Properties;

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
        new PropertyDefinition<string>(nameof(IPerson.FullAddress)).MappedGetter(() => address.FullAddress),
        new PropertyDefinition<string>(nameof(IPerson.Country)).Constant(address.Country)
      );
    }
  }
}