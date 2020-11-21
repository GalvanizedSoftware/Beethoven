using GalvanizedSoftware.Beethoven.Core.Properties.Instances;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class InitialParameterValue<T> : IPropertyDefinition<T>
  {
    private readonly bool valueSet;

    public IPropertyInstance<T> CreateInstance(object master) =>
      new InitialParameterValueInstance<T>();
  }
}