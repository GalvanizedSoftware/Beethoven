namespace GalvanizedSoftware.Beethoven.DemoApp.DuckTyping
{
  internal class Company
  {
    public Company(string name, string abbreviation)
    {
      Name = name;
      Abbreviation = abbreviation;
    }

    public string Name { get; }
    public string Abbreviation { get; }

    public string ShortName => Abbreviation;
    public string LongName => Name;

    public override string ToString()
    {
      return Name;
    }
  }
}
