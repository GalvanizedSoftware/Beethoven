using System;
using System.Linq;
using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Core.Events;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal sealed class EventAddInterceptor : IInterceptor
  {
    private readonly ActionEventInvoker actionEventNotifier;

    public EventAddInterceptor(ActionEventInvoker actionEventNotifier)
    {
      this.actionEventNotifier = actionEventNotifier;
    }

    public void Intercept(IInvocation invocation)
    {
      actionEventNotifier.Add((Delegate) invocation.Arguments.Single());
    }
  }
}