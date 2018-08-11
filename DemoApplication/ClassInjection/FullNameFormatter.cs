namespace GalvanizedSoftware.Beethoven.DemoApp.ClassInjection
{
  internal class FullNameFormatter
  {
    private readonly IPerson person;

    public FullNameFormatter(IPerson person)
    {
      this.person = person;
    }

    public string GetFullName(bool firstNameFirst, bool lastNameCapital, bool something)
    {
      string lastName = lastNameCapital ? person.LastName.ToUpper() : person.LastName;
      string firstName = person.FirstName;
      return firstNameFirst ?
        $"{firstName} {lastName}" :
        $"{lastName} {firstName}";
    }

    public string GetFullName(bool firstNameFirst, bool lastNameCapital)
    {
      string lastName = lastNameCapital ? person.LastName.ToUpper() : person.LastName;
      string firstName = person.FirstName;
      return firstNameFirst ?
        $"{firstName} {lastName}" :
        $"{lastName} {firstName}";
    }
  }
}
