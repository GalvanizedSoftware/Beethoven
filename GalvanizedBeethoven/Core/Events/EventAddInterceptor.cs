using System;
using System.Linq;
using Castle.DynamicProxy;

namespace GalvanizedSoftware.Beethoven.Core.Events
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