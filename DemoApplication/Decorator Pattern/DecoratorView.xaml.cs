namespace GalvanizedSoftware.Beethoven.DemoApp.Decorator_Pattern
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