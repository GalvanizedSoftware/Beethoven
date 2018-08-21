using GalvanizedSoftware.Beethoven.DemoApp.Common;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace GalvanizedSoftware.Beethoven.DemoApp.Decorator_Pattern
{
  internal class DecoratorViewModel
  {
    private readonly Factory factory = new Factory();

    public DecoratorViewModel()
    {
      AddLaptopCommand = new Command(() => OrderItems.Add(new Laptop()));
      AddMouseCommand = new Command(() => OrderItems.Add(new Mouse()));
      AddGiftWrapping = new Command(AddGiftWrappingOnAll);
    }

    private void AddGiftWrappingOnAll()
    {
      foreach (IOrderedItem item in OrderItems.ToList())
      {
        OrderItems.Remove(item);
        OrderItems.Add(factory.AddGiftWrapping(item));
      }
    }

    public ObservableCollection<IOrderedItem> OrderItems { get; } = new ObservableCollection<IOrderedItem>();

    public ICommand AddLaptopCommand { get; }

    public ICommand AddMouseCommand { get; }

    public ICommand AddGiftWrapping { get; }
  }
}