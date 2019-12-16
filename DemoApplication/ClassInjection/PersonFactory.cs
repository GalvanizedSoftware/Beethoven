namespace GalvanizedSoftware.Beethoven.DemoApp.ClassInjection
{
  internal class PersonFactory
  {
    public IPerson CreatePerson(string firstName, string lastName)
    {
      FullName fullName = new FullName
      {
        FirstName = firstName,
        LastName = lastName
      };
      return new TypeDefinition<IPerson>()
        .Add(fullName)
        .AddMethodMapper(main => new FullNameFormatter(main))
        .Create();
    }
  }
}