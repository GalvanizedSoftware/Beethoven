using System.ComponentModel;

namespace GalvanizedSoftware.Beethoven.DemoApp.ClassInjection
{
  public interface IPerson : INotifyPropertyChanged
  {
    string FirstName { get; set; }
    string LastName { get; set; }
    string GetFullName(bool firstNameFirst, bool lastNameCapital);
  }
}