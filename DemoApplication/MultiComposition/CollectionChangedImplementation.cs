using System;
using System.Collections.Specialized;
using GalvanizedSoftware.Beethoven.Core;

// ReSharper disable UnusedMember.Global

namespace GalvanizedSoftware.Beethoven.DemoApp.MultiComposition
{
  internal class CollectionChangedImplementation<T> : INotifyCollectionChanged, IBindingParent
  {
    private readonly Func<T, int> getIndex;
    private NotifyCollectionChangedEventHandler collectionChanged = delegate { };
    private int removedIndex;
    private object master;

    public CollectionChangedImplementation(Func<T, int> getIndex)
    {
      this.getIndex = getIndex;
      CollectionChanged += delegate { };
    }

    public event NotifyCollectionChangedEventHandler CollectionChanged
    {
      add => collectionChanged += value;
      // ReSharper disable once DelegateSubtraction
      remove => collectionChanged -= value;
    }

    public void Add(T item)
    {
      collectionChanged(master, new NotifyCollectionChangedEventArgs(
        NotifyCollectionChangedAction.Add,
        new[] { item },
        getIndex(item)));
    }

    public void Clear()
    {
      collectionChanged(master, new NotifyCollectionChangedEventArgs(
        NotifyCollectionChangedAction.Reset));
    }

    public bool PreRemove(T item)
    {
      removedIndex = getIndex(item);
      return removedIndex != -1;
    }

    public bool Remove(T item)
    {
      if (removedIndex == -1)
        return false;
      collectionChanged(master, new NotifyCollectionChangedEventArgs(
        NotifyCollectionChangedAction.Remove,
        new[] { item }, removedIndex));
      return true;
    }

    public void Bind(object target)
    {
      master = target;
    }
  }
}