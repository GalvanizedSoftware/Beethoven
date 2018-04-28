using System;
using System.Linq;
using Castle.DynamicProxy;

namespace GalvanizedSoftware.Beethoven.Core.Events
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