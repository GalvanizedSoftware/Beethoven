using GalvanizedSoftware.Beethoven.Generic.Events;
using System;

namespace GalvanizedSoftware.Beethoven.Core.Events
{
  public class EventTrigger : IEventTrigger
  {
    private readonly IGeneratedClass generatedClass;
    private readonly string eventName;

    public EventTrigger(object mainObject, string eventName)
    {
      generatedClass = mainObject as IGeneratedClass;
      this.eventName = eventName;
    }

    public object Notify(params object[] args) =>
      generatedClass.NotifyEvent(eventName, args);

    public Action ToAction() =>
      () => generatedClass.NotifyEvent(eventName, Array.Empty<object>());

    public Action<T> ToAction<T>() =>
      value => generatedClass.NotifyEvent(eventName, new object[] { value });

    public Action<T1, T2> ToAction<T1, T2>() =>
      (value1, value2) => generatedClass.NotifyEvent(eventName, new object[] { value1, value2 });

    public Action<T1, T2, T3> ToAction<T1, T2, T3>() =>
      (value1, value2, value3) => generatedClass.NotifyEvent(eventName, new object[] { value1, value2, value3 });

    public Func<TResult> ToFunc<TResult>() =>
      () => (TResult)generatedClass.NotifyEvent(eventName, Array.Empty<object>());

    public Func<T, TResult> ToFunc<T, TResult>() =>
      value => (TResult)generatedClass.NotifyEvent(eventName, new object[] { value });

    public Func<T1, T2, TResult> ToFunc<T1, T2, TResult>() =>
      (value1, value2) => (TResult)generatedClass.NotifyEvent(eventName, new object[] { value1, value2 });

    public Func<T1, T2, T3, TResult> ToFunc<T1, T2, T3, TResult>() =>
      (value1, value2, value3) => (TResult)generatedClass.NotifyEvent(eventName, new object[] { value1, value2, value3 });
  }
}
