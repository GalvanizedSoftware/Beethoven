namespace GalvanizedSoftware.Beethoven.DemoApp.Decorator
{
  public partial class DecoratorView
  {
    public DecoratorView()
    {
      DataContext = new DecoratorViewModel();
      InitializeComponent();
    }
  }
}