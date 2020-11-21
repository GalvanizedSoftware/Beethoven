using GalvanizedSoftware.Beethoven.Core.Properties.Instances;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class NotSupported<T> : IPropertyDefinition<T>
  {
    public IPropertyInstance<T> CreateInstance(object master) =>
      new NotSupportedInstance<T>();
  }
}