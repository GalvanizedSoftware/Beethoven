using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalvanizedSoftware.Beethoven.DemoApp.Properties;

namespace GalvanizedSoftware.Beethoven.DemoApp.Mapped
{
  public sealed class MappedViewModel : INotifyPropertyChanged
  {
    private IPerson person;

    public MappedViewModel()
    {
      UsAddress usAddress = new UsAddress
      {
        Adddress1 = "Washington ave. 22342",
        State = "MN",
        Zip = "55123",
        City = "Springfield"
      };
      Person = new PersonFactory().CreatePerson(usAddress);
      Person.FirstName = "John";
      Person.LastName = "Johnson";
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

    public event PropertyChangedEventHandler PropertyChanged;

    public void Update()
    {
      DkAddress dkAddress = new DkAddress
      {
        Adddress1 = "Vestergade 16",
        Zip = "2860",
        City = "Vinterbyøster"
      };
      IPerson newPerson = new PersonFactory().CreatePerson(dkAddress);
      newPerson.FirstName = "John";
      newPerson.LastName = "Johnson";
      Person = newPerson;
    }

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}