using System.Windows.Controls;
using System.Windows.Data;

namespace GalvanizedSoftware.Beethoven.DemoApp.MultiComposition
{
  public partial class MultiCompositionView
  {
    public MultiCompositionView()
    {
      MultiCompositionViewModel multiCompositionViewModel = new MultiCompositionViewModel();
      DataContext = multiCompositionViewModel;
      InitializeComponent();
    }
  }
}
