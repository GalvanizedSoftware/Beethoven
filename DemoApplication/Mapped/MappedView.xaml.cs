using System.Windows;

namespace GalvanizedSoftware.Beethoven.DemoApp.Mapped
{
  public partial class MappedView
  {
    private readonly MappedViewModel mappedViewModel = new MappedViewModel();

    public MappedView()
    {
      DataContext = mappedViewModel;
      InitializeComponent();
    }

    private void OnUpdate(object sender, RoutedEventArgs e)
    {
      mappedViewModel.Update();
    }
  }
}