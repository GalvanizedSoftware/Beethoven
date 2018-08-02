using GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdate.Server;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdate
{
  internal class ClientModel
  {
    private readonly ServerModel serverModel;

    public ClientModel(ServerModel serverModel)
    {
      this.serverModel = serverModel;
    }

    public ObservableCollection<IPerson> People { get; } = new ObservableCollection<IPerson>();

    public IPerson CreatePerson()
    {
      IPerson newPerson = serverModel.CreatePerson<IPerson>();
      newPerson.FirstName = "<FirstName>";
      newPerson.LastName = "<LastName>";
      newPerson.BirthDate = DateTime.Today.ToString(new CultureInfo(newPerson.Country, false));
      People.Add(newPerson);
      return newPerson;
    }
  }
}