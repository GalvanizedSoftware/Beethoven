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
    private static readonly ProxyGenerator generator = new ProxyGenerator();

    private readonly Dictionary<WeakReference, EventInvokers> generatedEventInvokers =
      new Dictionary<WeakReference, EventInvokers>();

    private static readonly MethodInfo generateMethodInfo = typeof(BeethovenFactory)
      .GetMethods()
      .Where(info => info.Name == nameof(Generate))
      .First(info => info.IsGenericMethod);

    public BeethovenFactory(params object[] generalPartDefinitions)
    {
      GeneralPartDefinitions = generalPartDefinitions;
    }

    public IEnumerable<object> GeneralPartDefinitions { get; set; }

    public object Generate(Type type, params object[] partDefinitions) =>
      generateMethodInfo
        .MakeGenericMethod(type)
        .Invoke(this, new object[] { partDefinitions });

    public T Generate<T>(params object[] partDefinitions) where T : class
    {
      partDefinitions = partDefinitions.Concat(GeneralPartDefinitions).ToArray();
      EventInvokers eventInvokers = new EventInvokers();
      return Create<T>(partDefinitions, Array.Empty<object>(), eventInvokers, 
        new WrapperFactories<T>(WrapperGenerator<T>.CreateAndCheckWrappers(partDefinitions), eventInvokers));
    }

    internal T Create<T>(object[] partDefinitions, object[] parameters,
      EventInvokers eventInvokers, IEnumerable<InterceptorMap> wrapperFactories)
      where T : class
    {
      InstanceContainer<T> instanceContainer = new InstanceContainer<T>(partDefinitions, parameters, eventInvokers, wrapperFactories);
      IInterceptor interceptor = instanceContainer.MasterInterceptor;
      T target = typeof(T).IsInterface
        ? generator.CreateInterfaceProxyWithoutTarget<T>(interceptor)
        : generator.CreateClassProxy<T>(interceptor);
      instanceContainer.Bind(target);
      generatedEventInvokers.Add(new WeakReference(target), eventInvokers);
      return target;
    }

    public static bool Implements<TInterface, TClass>() =>
      !new GeneralSignatureChecker(typeof(TInterface), typeof(TClass))
        .FindMissing()
        .Any();

    public static bool Implements<TInterface>(object instance) =>
      !new GeneralSignatureChecker(typeof(TInterface), instance?.GetType())
        .FindMissing()
        .Any();

    public IEventTrigger CreateEventTrigger(object mainObject, string name) =>
      generatedEventInvokers.Single(pair => pair.Key.Target == mainObject).Value[name];
  }
}