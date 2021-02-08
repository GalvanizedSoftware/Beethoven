using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
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

    public IPropertyInvoker<T> Create(object master) => 
      new InitialValueInvoker<T>(value);
  }
}