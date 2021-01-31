using GalvanizedSoftware.Beethoven.Core.Properties.Instances;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Properties
{
  internal class NotSupportedPropertyInvoker<T> : IPropertyInvoker<T>
  {
    public IPropertyInvokerInstance<T> Create(object _) =>
      new InvokerInstanceWrapper<T>(new NotSupportedInstance<T>());
  }
}