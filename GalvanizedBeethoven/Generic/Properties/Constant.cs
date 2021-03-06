﻿using System;
using GalvanizedSoftware.Beethoven.Core.Properties.Instances;
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

    public IPropertyInstance<T> Create(object master) => 
      new ConstantInstance<T>(value, errorHandler);
  }
}