using System;
using GalvanizedSoftware.Beethoven.Core.Properties.Instances;
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

    public IPropertyInstance<T> CreateInstance(object master) =>
      new LazyCreatorInstance<T>(valueCreator);
  }
}