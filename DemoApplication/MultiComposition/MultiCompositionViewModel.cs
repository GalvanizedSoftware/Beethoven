using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalvanizedSoftware.Beethoven.DemoApp.Common;
using GalvanizedSoftware.Beethoven.DemoApp.Properties;
using GalvanizedSoftware.Beethoven.Fluent;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Events;
using GalvanizedSoftware.Beethoven.Generic.Properties;

namespace GalvanizedSoftware.Beethoven.DemoApp.MultiComposition
{
  internal sealed class MultiCompositionViewModel : INotifyPropertyChanged
  {
    private readonly IPersonCollection personCollection;
    private readonly TypeDefinition<IPerson> personTypeDefinition =
      new TypeDefinition<IPerson>(
          new DefaultProperty().
            SkipIfEqual().
            SetterGetter().
            NotifyChanged());

    private IPerson selected;

    public MultiCompositionViewModel()
    {
      List<IPerson> persons = new List<IPerson>();
      RemovedLogger removedLogger = new RemovedLogger(person => persons.IndexOf(person));
      CollectionChangedImplementation<IPerson> collectionChanged =
        new CollectionChangedImplementation<IPerson>(() => personCollection, () => removedLogger.LastIndex);
      TypeDefinition<IPersonCollection> typeDefinition =
        new TypeDefinition<IPersonCollection>(
          new LinkedObjects(
            removedLogger,
            persons,
            collectionChanged));
      IEventTrigger trigger = null;
      typeDefinition.RegisterEvent(
        nameof(IPersonCollection.CollectionChanged),
        eventTrigger => trigger = eventTrigger);
      personCollection = typeDefinition.Create();
      collectionChanged.CollectionChanged += (sender, args) => trigger.Notify(sender, args);
      personCollection.CollectionChanged += (sender, args) => Trace.WriteLine($"sender: {sender}, type: {args.Action}");
      Items = personCollection;
      AddCommand = new Command(() => personCollection.Add(CreateNewPerson()));
      RemoveCommand = new Command(() => personCollection.Remove(Selected));
      ResetCommand = new Command(() => personCollection.Clear());
    }

    private IPerson CreateNewPerson()
    {
      IPerson newPerson = personTypeDefinition.Create();
      newPerson.FirstName = NewFirstName;
      newPerson.LastName = NewLastName;
      return newPerson;
    }

    public string NewFirstName { get; set; }

    public string NewLastName { get; set; }

    public ICommand AddCommand { get; }

    public ICommand RemoveCommand { get; }

    public ICommand ResetCommand { get; }

    public IEnumerable<IPerson> Items { get; }

    public IPerson Selected
    {
      get => selected;
      set
      {
        if (Equals(value, selected)) return;
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
