namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility2
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