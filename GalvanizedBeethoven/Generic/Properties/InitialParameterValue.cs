using GalvanizedSoftware.Beethoven.Core.Properties.Instances;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class InitialParameterValue<T> : IPropertyDefinition<T>
  {
    public IPropertyInstance<T> Create(object master) =>
      new InitialParameterValueInstance<T>();
  }
}