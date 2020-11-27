using Unity;

namespace GalvanizedSoftware.Beethoven.DemoApp.UnityContainer
{
  public partial class UnityView
  {
    private readonly IUnityContainer container = new Unity.UnityContainer();
    // ReSharper disable once NotAccessedField.Local
    private readonly Factory factory;

    public UnityView()
    {
      factory = new Factory(container);
      DataContext = container.Resolve<IUnityViewModel>();
      InitializeComponent();
    }
  }
}