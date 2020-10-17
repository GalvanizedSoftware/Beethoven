using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
using GalvanizedSoftware.Beethoven.Implementations.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;

namespace GalvanizedSoftware.Beethoven.Core.Invokers
{
  internal class NotSupportedPropertyInvoker<T> : IPropertyInvoker<T>
  {
    public IPropertyInvokerInstance<T> CreateInstance(object _) =>
      new InvokerInstanceWrapper<T>(new NotSupportedInstance<T>());
  }
}