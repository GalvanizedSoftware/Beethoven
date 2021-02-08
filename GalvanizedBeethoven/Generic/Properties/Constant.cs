using System;
using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class Constant<T> : IPropertyDefinition<T>
  {
    private readonly Action<T> errorHandler;
    private readonly T value;

    public Constant(T value) :
      this(value, null)
    {
    }

    public Constant(T value, Action<T> errorHandler)
    {
      this.value = value;
      this.errorHandler = errorHandler;
    }

    public IPropertyInvoker<T> Create(object master) => 
      new ConstantInvoker<T>(value, errorHandler);
  }
}