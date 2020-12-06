using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.StringComparison;

namespace GalvanizedSoftware.Beethoven
{
  public class AutoFactories
  {
    public static AutoFactories CreateFactories(Assembly assembly)
    {
      Type[] types = assembly?.GetTypes() ?? Array.Empty<Type>();
      (Type, Func<object>)[] factories =
        GetConstructorFactories(types)
          .Concat(GetMethodFactories(types))
          .ToArray();
      return factories.Length == 0 ? null : new AutoFactories(factories);
    }

    private const string InterfaceName = "GalvanizedSoftware.Beethoven.Interfaces.IFactoryDefinition";

    private AutoFactories((Type, Func<object>)[] factories)
    {
      Factories = factories;
    }

    public (Type, Func<object>)[] Factories { get; }

    public TypeDefinition<T> CreateTypeDefinition<T>() where T : class =>
      TypeDefinition<T>.CreateFromFactoryDefinition(Factories
        .FirstOrDefault(tuple => tuple.Item1 == typeof(T))
        .Item2?
        .Invoke() as IFactoryDefinition<T>);

    private static IEnumerable<(Type, Func<object>)> GetConstructorFactories(Type[] types) => types
              .Select(type => type.GetConstructor(Array.Empty<Type>()))
              .Where(IsFactory)
              .Select(constructorInfo =>
                (FindInterface(constructorInfo.DeclaringType), CreateFactory(constructorInfo)));

    private static IEnumerable<(Type, Func<object>)> GetMethodFactories(Type[] types) => types
              .SelectMany(type => type.GetMethods())
              .Where(IsFactory)
              .Where(methodInfo => methodInfo.GetParameters().Length == 0)
              .Select(methodInfo => (FindInterface(methodInfo.ReturnType), CreateFactory(methodInfo)));

    private static bool IsFactory(MemberInfo memberInfo) =>
      memberInfo?.GetCustomAttribute<FactoryAttribute>() != null;

    private static Func<object> CreateFactory(ConstructorInfo constructorInfo) =>
      () => constructorInfo.Invoke(Array.Empty<object>());

    private static Func<object> CreateFactory(MethodInfo methodInfo) =>
      () => methodInfo.Invoke(methodInfo.DeclaringType, Array.Empty<object>());

    private static Type FindInterface(Type type) =>
      type
        .GetInterfaces()
        .Concat(new[] { type })
        .FirstOrDefault(
          itemType => itemType
            .FullName?
            .StartsWith(InterfaceName, InvariantCulture) == true)?
        .GetGenericArguments()
        .FirstOrDefault();
  }
}
