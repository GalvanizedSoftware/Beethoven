using System;
using System.Linq;
using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Core.Events;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal sealed class EventRemoveInterceptor : IInterceptor
  {
    private readonly ActionEventInvoker actionEventNotifier;

    public EventRemoveInterceptor(ActionEventInvoker actionEventNotifier)
    {
      this.actionEventNotifier = actionEventNotifier;
    }

    public void Intercept(IInvocation invocation)
    {
      actionEventNotifier.Remove((Delegate) invocation.Arguments.Single());
    }
  }
}