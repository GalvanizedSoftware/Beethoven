namespace GalvanizedSoftware.Beethoven.DemoApp.MultiComposition
{
  public partial class MultiCompositionView
  {
    public MultiCompositionView()
    {
      DataContext = new MultiCompositionViewModel();
      InitializeComponent();
    }
  }
}
