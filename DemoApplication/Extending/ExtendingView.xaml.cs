namespace GalvanizedSoftware.Beethoven.DemoApp.Extending
{
  public partial class ExtendingView
  {
    public ExtendingView()
    {
      DataContext = new ExtendingViewModel();
      InitializeComponent();
    }
  }
}