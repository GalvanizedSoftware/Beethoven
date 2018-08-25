using System.ComponentModel;

namespace GalvanizedSoftware.Beethoven.DemoApp.UnityContainer
{
  public interface IPerson : INotifyPropertyChanged
  {
    string FirstName { get; set; }
    string LastName { get; set; }
  }
}