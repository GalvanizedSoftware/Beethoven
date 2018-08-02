namespace GalvanizedSoftware.Beethoven.DemoApp.Basic
{
  public partial class BasicView
  {
    public BasicView()
    {
      DataContext = new BasicViewModel();
      InitializeComponent();
    }
  }
}