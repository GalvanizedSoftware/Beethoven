namespace GalvanizedSoftware.Beethoven.Core.Binding
{
  internal class TargetBindingParent : IBindingParent
  {
    private readonly ObjectProviderBinder<ITargetBinding> objectProviderBinder;

    public TargetBindingParent(IObjectProvider objectProvider)
    {
      objectProviderBinder = new ObjectProviderBinder<ITargetBinding>(objectProvider);
    }

    public void Bind(object target)
    {
      objectProviderBinder.Bind(binding => binding.Bind(target));
    }
  }
}