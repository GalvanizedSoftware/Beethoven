using System.Linq;

namespace GalvanizedSoftware.Beethoven.DemoApp.DuckTyping
{
  internal class Person
  {
    public Person(string firstName, string lastName)
    {
      FirstName = firstName;
      LastName = lastName;
    }

    public string FirstName { get; }
    public string LastName { get; }

    public string ShortName => $"{FirstName.FirstOrDefault()}. {LastName.FirstOrDefault()}.";
    public string LongName => FirstName + " " + LastName;

    public override string ToString()
    {
      return FirstName + " " + LastName;
    }
  }
}
