namespace GalvanizedSoftware.Beethoven.Core.Invokers
{
  public class PropertyInvoker<T> : IPropertyInvoker<T>
  {
    private readonly IPropertyInvoker<T> masterInvoker;

    public PropertyInvoker(string uniqueName)
    {
      masterInvoker = InvokerList.GetInvoker(uniqueName) as IPropertyInvoker<T> ??
        new NotImplementedPropertyInvoker<T>();
    }

    public T InvokeGet(object master) => masterInvoker.InvokeGet(master);

    public void InvokeSet(object master, T newValue) => masterInvoker.InvokeSet(master, newValue);
  }
}
