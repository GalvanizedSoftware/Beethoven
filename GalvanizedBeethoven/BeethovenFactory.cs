using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Events;
using GalvanizedSoftware.Beethoven.Generic.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven
{
  public sealed class BeethovenFactory
  {
    public object[] GeneralPartDefinitions { get; set; }
    private static readonly ProxyGenerator generator = new ProxyGenerator();

    private readonly Dictionary<WeakReference, EventInvokers> generatedEventInvokers =
      new Dictionary<WeakReference, EventInvokers>();

    private static readonly MethodInfo generateMethodInfo;

    static BeethovenFactory()
    {
      generateMethodInfo = typeof(BeethovenFactory)
        .GetMethods()
        .Where(info => info.Name == nameof(Generate))
        .First(info => info.IsGenericMethod);
    }

    public BeethovenFactory(params object[] generalPartDefinitions)
    {
      GeneralPartDefinitions = generalPartDefinitions;
    }

    public object Generate(Type type, params object[] partDefinitions) =>
      generateMethodInfo
        .MakeGenericMethod(type)
        .Invoke(this, new object[] { partDefinitions });

    public T Generate<T>(params object[] partDefinitions) where T : class
    {
      partDefinitions = partDefinitions.Concat(GeneralPartDefinitions).ToArray();
      return Create<T>(partDefinitions, 
        WrapperGenerator<T>.GetWrappers(partDefinitions).ToList(), 
        new object[0]);
    }

    internal T Create<T>(object[] partDefinitions, List<object> wrappers, object[] parameters)
      where T : class
    {
      InstanceContainer<T> instanceContainer =
        new InstanceContainer<T>(partDefinitions, wrappers, parameters);
      IInterceptor interceptor = instanceContainer.MasterInterceptor;
      T target = typeof(T).IsInterface
        ? generator.CreateInterfaceProxyWithoutTarget<T>(interceptor)
        : generator.CreateClassProxy<T>(interceptor);
      instanceContainer.Bind(target);
      generatedEventInvokers.Add(new WeakReference(target), instanceContainer.EventInvokers);
      return target;
    }

    public bool Implements<TInterface, TClass>() =>
      !new GeneralSignatureChecker(typeof(TInterface), typeof(TClass))
        .FindMissing()
        .Any();

    public bool Implements<TInterface>(object instance) =>
      !new GeneralSignatureChecker(typeof(TInterface), instance.GetType())
        .FindMissing()
        .Any();

    public IEventTrigger CreateEventTrigger(object mainObject, string name) =>
      generatedEventInvokers.Single(pair => pair.Key.Target == mainObject).Value[name];
  }
}