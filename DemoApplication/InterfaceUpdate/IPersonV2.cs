using System;
using System.ComponentModel;

// ReSharper disable once CheckNamespace
namespace GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdateV2
{
  public interface IPerson : INotifyPropertyChanged
  {
    string FirstName { get; set; }
    string LastName { get; set; }
    DateTime BirthDate{ get;set; }
  }
}