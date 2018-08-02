using System.Windows;

namespace GalvanizedSoftware.Beethoven.DemoApp.ClassInjection
{
  public partial class ClassInjectionView
  {
    private readonly ClassInjectionViewModel viewModel = new ClassInjectionViewModel();

    public ClassInjectionView()
    {
      DataContext = viewModel;
      InitializeComponent();
    }

    private void OnGetFullName(object sender, RoutedEventArgs e)
    {
      viewModel.GetFullName();
    }
  }
}