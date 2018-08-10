using System.Diagnostics;

namespace GalvanizedSoftware.Beethoven.DemoApp.Default
{
  internal class DefaultImplementaionViewModel
  {
    public DefaultImplementaionViewModel()
    {
      Person = new PersonFactory().CreatePerson();
      Person.PropertyChanged += (sender, args) => Trace.WriteLine($"PropertyChanged: {args.PropertyName}");
      Person.FirstName = "John";
      Trace.WriteLine($"FirstName is {Person.FirstName}");
    }

    public IPerson Person { get; }
  }
}