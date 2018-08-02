using System.ComponentModel;

namespace GalvanizedSoftware.Beethoven.DemoApp.DefaultImplementaion
{
  public interface IPerson : INotifyPropertyChanged
  {
    string FirstName { get; set; }
    string LastName { get; set; }
    string Address { get; set; }
  }
}