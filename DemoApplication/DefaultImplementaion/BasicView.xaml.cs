namespace GalvanizedSoftware.Beethoven.DemoApp.DefaultImplementaion
{
  public partial class DefaultImplementaionView
  {
    public DefaultImplementaionView()
    {
      DataContext = new DefaultImplementaionViewModel();
      InitializeComponent();
    }
  }
}