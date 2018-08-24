namespace GalvanizedSoftware.Beethoven.DemoApp.DuckTyping
{
  public partial class DuckTypingView
  {
    public DuckTypingView()
    {
      DataContext = new DuckTypingViewModel();
      InitializeComponent();
    }
  }
}