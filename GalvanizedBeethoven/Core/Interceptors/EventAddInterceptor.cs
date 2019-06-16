using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Events;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal sealed class EventAddInterceptor : IGeneralInterceptor
  {
    private readonly ActionEventInvoker actionEventNotifier;

    public EventAddInterceptor(ActionEventInvoker actionEventNotifier)
    {
      MainDefinition = this.actionEventNotifier = actionEventNotifier;
    }

    public void Invoke(InstanceMap instanceMap, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo)
    {
      actionEventNotifier.Add((Delegate)parameters.Single());
    }

    public object MainDefinition { get; }
  }
}