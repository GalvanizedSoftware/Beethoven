using System.ComponentModel;

namespace GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdate.InterfacesV1
{
  public interface IPerson : INotifyPropertyChanged
  { 
    string FirstName { get; set; }
    string LastName { get; set; }
    string Country { get; }
    string BirthDate { get;set;}
  }
}