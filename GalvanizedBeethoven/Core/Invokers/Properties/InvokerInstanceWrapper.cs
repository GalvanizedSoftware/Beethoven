using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  internal class InvokerInstanceWrapper<T> : IPropertyInvokerInstance<T>
  {
    private readonly IPropertyInstance<T> propertyInstance;

    public InvokerInstanceWrapper(IPropertyInstance<T> propertyInstance)
    {
      this.propertyInstance = propertyInstance;
    }

    public T InvokeGetter()
    {
      T value = default(T);
      propertyInstance.InvokeGetter(ref value);
      return value;
    }

    public void InvokeSetter(T newValue) => 
      propertyInstance.InvokeSetter(newValue);
  }
}
