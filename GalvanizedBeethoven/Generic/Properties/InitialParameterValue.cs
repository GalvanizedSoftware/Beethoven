using GalvanizedSoftware.Beethoven.Implementations.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class InitialParameterValue<T> : IPropertyDefinition<T>
  {
    private bool valueSet;

    public IPropertyInstance<T> CreateInstance(object master) =>
      new InitialParameterValueInstance<T>();
  }
}