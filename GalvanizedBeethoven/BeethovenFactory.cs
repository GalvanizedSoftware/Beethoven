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
    private readonly Dictionary<WeakReference, EventInvokers> generatedObjects = new Dictionary<WeakReference, EventInvokers>();
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

    public object Generate(Type type, params object[] partDefinitions)
    {
      MethodInfo makeGenericMethod = generateMethodInfo
        .MakeGenericMethod(type);
      return makeGenericMethod
        .Invoke(this, new object[] { partDefinitions });
    }

    public T Generate<T>(params object[] partDefinitions) where T : class
    {
      partDefinitions = partDefinitions.Concat(GeneralPartDefinitions).ToArray();
      InstanceContainer<T> instanceContainer = new InstanceContainer<T>(partDefinitions);
      IInterceptor interceptor = instanceContainer.GetMaster<IInterceptor>();
      T target = typeof(T).IsInterface ?
        generator.CreateInterfaceProxyWithoutTarget<T>(interceptor) :
        generator.CreateClassProxy<T>(interceptor);
      instanceContainer.Bind(target);
      generatedObjects.Add(new WeakReference(target), instanceContainer.EventInvokers);
      return target;
    }

    public bool Implements<TInterface, TClass>()
    {
      GeneralSignatureChecker<TInterface, TClass> signatureChecker = new GeneralSignatureChecker<TInterface, TClass>();
      return !signatureChecker.FindMissing().Any();
    }

    public IEventTrigger CreateEventTrigger(object mainObject, string name)
    {
      return generatedObjects.Single(pair => pair.Key.Target == mainObject).Value[name];
    }
  }
}