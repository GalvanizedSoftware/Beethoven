using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Invokers.Properties;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Factories
{
  internal static class PropertyInvokerFactory
  {
    private static readonly MethodInfo createMethod = 
      typeof(PropertyInvokerFactory)
        .GetMethods(ReflectionConstants.StaticResolveFlags)
        .Where(info => info.Name == nameof(Create))
        .First(info => info.IsGenericMethod);

    public static object Create(Type type, IEnumerable<object> definitions) =>
      createMethod
        .MakeGenericMethod(type)
        .Invoke(type, new object[] { definitions });

    private static object Create<T>(IEnumerable<object> definitions) =>
      new CompositePropertyInvoker<T>(definitions
        .OfType<IPropertyDefinition<T>>()
        .ToArray());
  }
}
