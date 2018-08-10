namespace GalvanizedSoftware.Beethoven.DemoApp.Default
{
  public partial class DefaultView
  {
    public DefaultView()
    {
      DataContext = new DefaultImplementaionViewModel();
      InitializeComponent();
    }
  }
}