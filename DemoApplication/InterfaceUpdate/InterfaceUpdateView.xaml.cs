using GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdate.Server;
using System.Windows;

namespace GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdate
{
  public partial class InterfaceUpdateView
  {
    readonly ClientModel clientModel;

    public InterfaceUpdateView()
    {
      InitializeComponent();
      ServerModel serverModel = new ServerModel();
      ClientViewModel clientViewModel = new ClientViewModel(serverModel);
      clientModel = clientViewModel.ClientModel;
      clientView.DataContext = clientModel;
      serverView.DataContext = serverModel;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      clientModel.CreatePerson();
    }
  }
}