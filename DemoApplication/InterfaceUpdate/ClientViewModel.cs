using GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdate.Server;

namespace GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdate
{
  class ClientViewModel
  {
    // ReSharper disable once NotAccessedField.Local
    private readonly ServerModel serverModel;

    public ClientModel ClientModel { get; }

    public ClientViewModel(ServerModel serverModel)
    {
      this.serverModel = serverModel;
      ClientModel = new ClientModel(serverModel);
    }

    public void CreatePerson()
    {
      ClientModel.CreatePerson();
    }
  }
}
