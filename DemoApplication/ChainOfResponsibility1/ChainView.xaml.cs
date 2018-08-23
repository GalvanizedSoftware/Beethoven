namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility1
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