using System.ComponentModel;

namespace GalvanizedSoftware.Beethoven.DemoApp.Mapped
{
  public interface IPerson : INotifyPropertyChanged
  {
    string FirstName { get; set; }
    string LastName { get; set; }
    string Country { get; }
    string FullAddress { get; }
    string GetFullName(bool firstNameFirst, bool lastNameCapital);
  }
}