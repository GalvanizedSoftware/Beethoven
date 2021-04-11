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
      MemberInfo[] factoryMethods =
        GetConstructorFactories(types)
          .Concat(GetMethodFactories(types))
          .ToArray();
      (Type, object, MemberInfo)[] factories = factoryMethods
        .Select(CreateFactory)
        .ToArray();
      return factories.Length == 0 ? null : new AutoFactories(factories);
    }

    private const string InterfaceName = "GalvanizedSoftware.Beethoven.Interfaces.IFactoryDefinition";

    private AutoFactories((Type, object, MemberInfo)[] factories)
    {
      Factories = factories;
    }

    public (Type, object, MemberInfo)[] Factories { get; }

    public TypeDefinition<T> CreateTypeDefinition<T>() where T : class
    {
      (_, object factoryDefinition, MemberInfo memberInfo) = Factories
        .FirstOrDefault(tuple => tuple.Item1 == typeof(T));
      return TypeDefinition<T>.CreateFromFactoryDefinition(
        factoryDefinition as IFactoryDefinition<T>,
        memberInfo);
    }

    private static (Type, object, MemberInfo) CreateFactory(MemberInfo memberInfo) =>
      memberInfo switch
      {
        ConstructorInfo constructorInfo =>
          (FindInterface(constructorInfo.DeclaringType), CreateFactory(constructorInfo), memberInfo),
        MethodInfo methodInfo =>
          (FindInterface(methodInfo.ReturnType), CreateFactory(methodInfo), memberInfo),
        _ => default
      };

    private static IEnumerable<MemberInfo> GetConstructorFactories(Type[] types) => types
              .Select(type => type.GetConstructor(Array.Empty<Type>()))
              .Where(IsFactory);

    private static IEnumerable<MemberInfo> GetMethodFactories(Type[] types) => types
              .SelectMany(type => type.GetMethods())
              .Where(IsFactory)
              .Where(methodInfo => methodInfo.GetParameters().Length == 0);

    private static bool IsFactory(MemberInfo memberInfo) =>
      memberInfo?.GetCustomAttribute<FactoryAttribute>() != null;

    private static object CreateFactory(ConstructorInfo constructorInfo) =>
      constructorInfo.Invoke(Array.Empty<object>());

    private static object CreateFactory(MethodInfo methodInfo) =>
      methodInfo.Invoke(methodInfo.DeclaringType, Array.Empty<object>());

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
