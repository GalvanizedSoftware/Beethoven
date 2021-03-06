﻿using System;
using System.ComponentModel;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Properties.Instances
{
  internal class NotifyChangedInstance<T> : IPropertyInstance<T>
  {
    private const string PropertyChangedName = nameof(INotifyPropertyChanged.PropertyChanged);
    private readonly PropertyChangedEventArgs eventArgs;
    private readonly IGeneratedClass master;

    public NotifyChangedInstance(object master, string name)
    {
      this.master = master as IGeneratedClass ?? throw new NullReferenceException();
      eventArgs = new PropertyChangedEventArgs(name);
    }

    public bool InvokeGetter(ref T __) => true;

    public bool InvokeSetter(T newValue)
    {
      master.NotifyEvent(PropertyChangedName, new object[] { master, eventArgs });
      return true;
    }
  }
}