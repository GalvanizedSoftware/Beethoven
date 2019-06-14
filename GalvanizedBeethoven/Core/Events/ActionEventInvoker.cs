using GalvanizedSoftware.Beethoven.Generic.Events;
using System;
using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Interceptors;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Events
{
  public sealed class ActionEventInvoker : IEventTrigger, IInterceptorProvider
  {
    private readonly string name;
    private Delegate delegateList;

    public ActionEventInvoker(string name)
    {
      this.name = name;
    }

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

    public IEnumerable<InterceptorMap> GetInterceptorMaps<T>()
    {
      EventInfo eventInfo = typeof(T).GetEventInfo(name);
      if (eventInfo == null)
        yield break;
      yield return new InterceptorMap(eventInfo.AddMethod, new EventAddInterceptor(this));
      yield return new InterceptorMap(eventInfo.RemoveMethod, new EventRemoveInterceptor(this));
    }
  }
}