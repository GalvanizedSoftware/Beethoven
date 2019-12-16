using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Events;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal sealed class EventRemoveInterceptor : IGeneralInterceptor
  {
    private readonly ActionEventInvoker actionEventNotifier;

    public EventRemoveInterceptor(ActionEventInvoker actionEventNotifier)
    {
      MainDefinition = this.actionEventNotifier = actionEventNotifier;
    }

    public void Invoke(InstanceMap instanceMap, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo)
    {
      actionEventNotifier.Remove((Delegate)parameters.Single());
    }

    public object MainDefinition { get; }
  }
}