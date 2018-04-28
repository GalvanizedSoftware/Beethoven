using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using GalvanizedSoftware.Beethoven.Core.Binding;
using GalvanizedSoftware.Beethoven.Core.Events;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class InstanceContainer<T> : IObjectProvider where T : class
  {
    private readonly Dictionary<Type, object> masters = new Dictionary<Type, object>();
    private readonly IObjectProvider objectProviderHandler;

    public InstanceContainer(object[] partDefinitions)
    {
      AddMaster(new TargetBindingParent(this));
      EventInvokers eventInvokers = AddMaster(new EventInvokers(this));
      object[] wrappers = GetWrappers(partDefinitions).ToArray();
      MasterInterceptor masterInterceptor = new MasterInterceptor(
        new PropertiesFactory(wrappers.OfType<Property>()),
        new MethodsFactory(wrappers.OfType<Method>()),
        new EventsFactory<T>(eventInvokers));
      AddMaster<IInterceptor>(masterInterceptor);
      objectProviderHandler = new ObjectProviderHandler(
        masters.Values.Concat(
        partDefinitions.OfType<IBindingParent>()));
    }

    public IEnumerable<TChild> Get<TChild>()
    {
      return objectProviderHandler.Get<TChild>();
    }

    private static IEnumerable<object> GetWrappers(object[] partDefinitions)
    {
      foreach (object definition in partDefinitions)
      {
        switch (definition)
        {
          case null:
            continue;
          case Property property:
            yield return property;
            break;
          case Method method:
            yield return method;
            break;
          case IEnumerable<Property> properties:
            foreach (Property property in properties)
              yield return property;
            break;
          case IEnumerable<Method> methods:
            foreach (Method method in methods)
              yield return method;
            break;
          default:
            foreach (Property subProperty in new PropertyMapper(definition))
              yield return subProperty;
            // Methods
            break;
        }
      }
    }

    internal void Bind(T target)
    {
      foreach (IBindingParent bindingParent in Get<IBindingParent>())
        bindingParent.Bind(target);
    }

    internal TMaster GetMaster<TMaster>()
    {
      object value;
      if (masters.TryGetValue(typeof(TMaster), out value))
        return (TMaster)value;
      return default(TMaster);
    }

    private TMaster AddMaster<TMaster>(TMaster instance)
    {
      masters.Add(typeof(TMaster), instance);
      return instance;
    }
  }
}