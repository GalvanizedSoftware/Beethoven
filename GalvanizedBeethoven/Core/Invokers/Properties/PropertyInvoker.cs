using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers
{
  public class PropertyInvoker<T> : IPropertyInvoker<T>
  {
    private readonly IPropertyInvoker<T> masterInvoker;

    public PropertyInvoker(string uniqueName)
    {
      masterInvoker = InvokerList.CreateInvoker(uniqueName) as IPropertyInvoker<T> ??
        new NotSupportedPropertyInvoker<T>();
    }

    public IPropertyInvokerInstance<T> CreateInstance(object master) => 
      masterInvoker.CreateInstance(master);
  }
}
