namespace GalvanizedSoftware.Beethoven.DemoApp.DuckTyping
{
  internal class Company
  {
    public Company(string name, string abreviation)
    {
      Name = name;
      Abreviation = abreviation;
    }

    public string Name { get; }
    public string Abreviation { get; }

    public string ShortName => Abreviation;
    public string LongName => Name;

    public override string ToString()
    {
      return Name;
    }
  }
}
