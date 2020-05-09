using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Invokers.Factories
{
  internal static class InvokerFactory
  {
    private static readonly MethodInfo createPropertyInvokerMethod = typeof(InvokerFactory)
      .GetMethods()
      .Where(info => info.Name == nameof(CreatePropertyInvoker))
      .First(info => info.IsGenericMethod);

    public static object CreatePropertyInvoker(Type type, IEnumerable<object> definitions) =>
      createPropertyInvokerMethod
        .MakeGenericMethod(type)
        .Invoke(type, new[] { definitions });

    public static CompositePropertyInvoker<T> CreatePropertyInvoker<T>(IEnumerable<object> definitions) =>
      new CompositePropertyInvoker<T>(definitions
        .OfType<IPropertyDefinition<T>>()
        .ToArray());

    internal static IMethodInvoker CreateMethodInvoker(MethodInfo methodInfo, MethodDefinition methodDefinition) =>
      new RealMethodInvoker(methodInfo, methodDefinition);
  }
}
