using System.Diagnostics;

namespace GalvanizedSoftware.Beethoven.DemoApp.Basic
{
  internal class BasicViewModel
  {
    public BasicViewModel()
    {
      Person = new PersonFactory().CreatePerson();
      Person.PropertyChanged += (sender, args) => Trace.WriteLine($"PropertyChanged: {args.PropertyName}");
      Person.FirstName = "Lisa";
      Trace.WriteLine($"FirstName is {Person.FirstName}");
    }

    public IPerson Person { get; }
  }
}