using System.Collections.ObjectModel;

namespace GalvanizedSoftware.Beethoven.DemoApp.DuckTyping
{
  internal class DuckTypingViewModel
  {
    private readonly Factory factory = new Factory();

    public DuckTypingViewModel()
    {
      AddObject(new Person("John", "Cheese"));
      AddObject(new Company("Galvanized Software", "GAS"));
      AddObject(new Person("Jane", "Madsen"));
      AddObject(new Company("Microsoft", "MS"));
    }

    public ObservableCollection<object> SomeObjects { get; } = new ObservableCollection<object>();

    public ObservableCollection<IDisplayName> DisplayNames { get; } = new ObservableCollection<IDisplayName>();

    private void AddObject(object something)
    {
      SomeObjects.Add(something);
      DisplayNames.Add(factory.CreateDisplayName(something));
    }
  }
}