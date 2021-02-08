using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class InitialParameterValue<T> : IPropertyDefinition<T>
  {
    public IPropertyInvoker<T> Create(object master) =>
      new InitialParameterValueInvoker<T>();
  }
}