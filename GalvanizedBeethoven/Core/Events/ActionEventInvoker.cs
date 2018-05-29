using GalvanizedSoftware.Beethoven.Generic.Events;
using System;

namespace GalvanizedSoftware.Beethoven.Core.Events
{
  public sealed class ActionEventInvoker : IEventTrigger
  {
    private Delegate delegateList;

    public object Notify(params object[] args)
    {
      return delegateList?.DynamicInvoke(args);
    }

    public void Add(Delegate newDelegate)
    {
      delegateList = Delegate.Combine(delegateList, newDelegate);
    }

    public void Remove(Delegate oldDelegate)
    {
      delegateList = Delegate.Remove(delegateList, oldDelegate);
    }
  }
}