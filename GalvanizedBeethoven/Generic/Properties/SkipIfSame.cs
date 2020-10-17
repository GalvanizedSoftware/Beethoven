using GalvanizedSoftware.Beethoven.Implementations.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  internal class SkipIfEqual<T> : IPropertyDefinition<T>
  {
    public IPropertyInstance<T> CreateInstance(object master) =>
      new SkipIfEqualInstance<T>();
  }
}