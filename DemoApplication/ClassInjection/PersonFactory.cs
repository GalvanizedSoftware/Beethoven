using GalvanizedSoftware.Beethoven.Generic;

namespace GalvanizedSoftware.Beethoven.DemoApp.ClassInjection
{
  internal class PersonFactory
  {
    private readonly BeethovenFactory factory = new BeethovenFactory();

    public IPerson CreatePerson(string firstName, string lastName)
    {
      FullName fullName = new FullName
      {
        FirstName = firstName,
        LastName = lastName
      };
      FactoryHelper<IPerson> factoryHelper = new FactoryHelper<IPerson>();
      return factory.Generate<IPerson>(
        fullName,
        factoryHelper.MethodMapper(main => new FullNameFormatter(main))
      );
    }
  }
}