using System.Diagnostics;

namespace GalvanizedSoftware.Beethoven.DemoApp.UnityContainer
{
  public class UnityViewModel
  {
    public UnityViewModel(IPerson person)
    {
      Person = person;
      Person.PropertyChanged += (sender, args) => Trace.WriteLine($"PropertyChanged: {args.PropertyName}");
      Person.FirstName = "Lisa";
      Trace.WriteLine($"FirstName is {Person.FirstName}");
    }

    public IPerson Person { get; }
  }
}