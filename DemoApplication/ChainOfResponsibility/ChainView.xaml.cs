namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility
{
  public partial class ChainView
  {
    public ChainView()
    {
      DataContext = new ChainViewModel();
      InitializeComponent();
    }
  }
}