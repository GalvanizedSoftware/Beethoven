using Unity;

namespace GalvanizedSoftware.Beethoven.DemoApp.UnityContainer
{
  public partial class UnityView
  {
    private readonly IUnityContainer container = new Unity.UnityContainer();
    private Factory factory;

    public UnityView()
    {
      factory = new Factory(container);
      DataContext = container.Resolve<UnityViewModel>();
      InitializeComponent();
    }
  }
}