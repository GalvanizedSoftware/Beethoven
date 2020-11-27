using GalvanizedSoftware.Beethoven.Core.Properties.Instances;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class InitialValue<T> : IPropertyDefinition<T>
  {
    private readonly T value;

    public InitialValue(T value)
    {
      this.value = value;
    }

    public IPropertyInstance<T> CreateInstance(object master) => 
      new InitialValueInstance<T>(value);
  }
}