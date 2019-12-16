using System.ComponentModel;

namespace GalvanizedSoftware.Beethoven.Test.CompositeTests.Interfaces
{
  public interface IPerson : INotifyPropertyChanged
  {
    string FirstName { get; set; }
    string LastName { get; set; }
  }
}