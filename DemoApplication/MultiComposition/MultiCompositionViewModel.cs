using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalvanizedSoftware.Beethoven.DemoApp.Common;
using GalvanizedSoftware.Beethoven.DemoApp.Properties;

namespace GalvanizedSoftware.Beethoven.DemoApp.MultiComposition
{
  internal sealed class MultiCompositionViewModel : INotifyPropertyChanged
  {
    private IPerson selected;
    private readonly Factory factory = new Factory();

    public MultiCompositionViewModel()
    {
      IPersonCollection personCollection = factory.CreatePersonCollection();
      personCollection.CollectionChanged += (sender, args) => Trace.WriteLine($"sender: {sender}, type: {args.Action}");
      Items = personCollection;
      AddCommand = new Command(() => personCollection.Add(CreateNewPerson()));
      RemoveCommand = new Command(() => personCollection.Remove(Selected));
      ResetCommand = new Command(() => personCollection.Clear());
    }

    private IPerson CreateNewPerson()
    {
      IPerson newPerson = factory.CreatePerson();
      newPerson.FirstName = NewFirstName;
      newPerson.LastName = NewLastName;
      return newPerson;
    }

    public string NewFirstName { get; set; } = "";

    public string NewLastName { get; set; } = "";

    public ICommand AddCommand { get; }

    public ICommand RemoveCommand { get; }

    public ICommand ResetCommand { get; }

    public IEnumerable<IPerson> Items { get; }

    public IPerson Selected
    {
      get => selected;
      set
      {
        if (Equals(value, selected))
          return;
        selected = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
