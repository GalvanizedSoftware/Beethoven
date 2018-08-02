using System.ComponentModel;

// ReSharper disable once CheckNamespace
namespace GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdateV1
{
  public interface IPerson : INotifyPropertyChanged
  { 
    string FirstName { get; set; }
    string LastName { get; set; }
    string Country { get; }
    string BirthDate { get;set;}
  }
}