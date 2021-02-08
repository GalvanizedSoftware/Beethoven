using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  internal class SkipIfEqual<T> : IPropertyDefinition<T>
  {
    public IPropertyInvoker<T> Create(object master) =>
      new SkipIfEqualInvoker<T>();
  }
}