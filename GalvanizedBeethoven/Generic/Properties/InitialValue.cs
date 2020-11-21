﻿using GalvanizedSoftware.Beethoven.Implementations.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public class InitialValue<T> : IPropertyDefinition<T>
  {
    private readonly T value;
    private readonly bool valueSet;

    public InitialValue(T value)
    {
      this.value = value;
    }

    public IPropertyInstance<T> CreateInstance(object master) => 
      new InitialValueInstance<T>(value);
  }
}