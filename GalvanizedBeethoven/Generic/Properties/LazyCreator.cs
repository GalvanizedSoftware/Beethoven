using System;
using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class LazyCreator<T> : IPropertyDefinition<T>
  {
    private readonly Func<T> valueCreator;

    public LazyCreator(Func<T> valueCreator)
    {
      this.valueCreator = valueCreator;
    }

    public IPropertyInvoker<T> Create(object master) =>
      new LazyCreatorInvoker<T>(valueCreator);
  }
}