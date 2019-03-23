using System;
using System.Collections.Specialized;
// ReSharper disable UnusedMember.Global

namespace GalvanizedSoftware.Beethoven.DemoApp.MultiComposition
{
  internal class CollectionChangedImplementation<T> : INotifyCollectionChanged
  {
    private readonly Func<int> getRemovedIndex;
    private readonly Func<object> masterGetter;
    private NotifyCollectionChangedEventHandler collectionChanged = delegate { };

    public CollectionChangedImplementation(Func<object> masterGetter, Func<int> getRemovedIndex)
    {
      this.getRemovedIndex = getRemovedIndex;
      this.masterGetter = masterGetter ?? (() => this);
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
      collectionChanged(masterGetter(), new NotifyCollectionChangedEventArgs(
        NotifyCollectionChangedAction.Add,
        new[] { item }));
    }

    public void Clear()
    {
      collectionChanged(masterGetter(), new NotifyCollectionChangedEventArgs(
        NotifyCollectionChangedAction.Reset));
    }

    public bool Remove(T item)
    {
      int index = getRemovedIndex();
      if (index==-1)
        return false;
      collectionChanged(masterGetter(), new NotifyCollectionChangedEventArgs(
        NotifyCollectionChangedAction.Remove,
        new[] { item }, index));
      return true;
    }
  }
}