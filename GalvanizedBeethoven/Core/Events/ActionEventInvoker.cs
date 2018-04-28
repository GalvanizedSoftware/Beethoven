using System;

namespace GalvanizedSoftware.Beethoven.Core.Events
{
  public sealed class ActionEventInvoker
  {
    private Delegate delegateList;

    internal object Notify(object[] args)
    {
      if (delegateList == null)
        return null;
      return delegateList.DynamicInvoke(args);
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