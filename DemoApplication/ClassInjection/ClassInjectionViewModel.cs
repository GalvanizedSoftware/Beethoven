using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalvanizedSoftware.Beethoven.DemoApp.Properties;

namespace GalvanizedSoftware.Beethoven.DemoApp.ClassInjection
{
  public sealed class ClassInjectionViewModel : INotifyPropertyChanged
  {
    private IPerson person;
    private string fullName;

    public ClassInjectionViewModel()
    {
      Person = new PersonFactory().CreatePerson("John", "Johnson");
    }

    public IPerson Person
    {
      get => person;
      set
      {
        if (Equals(value, person)) return;
        person = value;
        OnPropertyChanged();
      }
    }

    public string FullName
    {
      get => fullName;
      set
      {
        if (value == fullName) return;
        fullName = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void GetFullName()
    {
      FullName = person.GetFullName(false, true);
    }

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}