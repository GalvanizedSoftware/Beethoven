using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Build
{
  internal class CodeGenerator
  {
    private static readonly MethodInfo generateCodeInternalMethod;
      //= typeof(BeethovenFactory)
      //.GetMethods(ReflectionConstants.ResolveFlags)
      //.Where(info => info.Name == nameof(GenerateCode))
      //.First(info => info.IsGenericMethod);
    private readonly object factory;
    private readonly MethodInfo generateMethod;

    static CodeGenerator()
    {
      MethodInfo[] methodInfos = typeof(CodeGenerator)
        .GetMethods(ReflectionConstants.ResolveFlags);
      IEnumerable<MethodInfo> enumerable = methodInfos
        .Where(info => info.Name == nameof(GenerateCode));
      generateCodeInternalMethod = enumerable
        .FirstOrDefault(info => info.IsGenericMethod);
    }

    internal static CodeGenerator Create(Type type, object factory) => 
      new CodeGenerator(factory, generateCodeInternalMethod.MakeGenericMethod(type));
    private CodeGenerator(object factory, MethodInfo generateMethod)
    {
      this.factory = factory;
      this.generateMethod = generateMethod;
    }

    public (string filename, string code) GenerateCode() =>
      ((string filename, string code))generateMethod.Invoke(this, new object[0]);

    private (string filename, string code) GenerateCode<T>() where T : class
    {
      IFactoryDefinition<T> factoryDefinition = factory as IFactoryDefinition<T>;
      TypeDefinition<T> typeDefinition = TypeDefinition<T>
        .CreateFromFactoryDefinition(factoryDefinition);
      if (factoryDefinition == null)
        return (null, null);
      return
      (
        factoryDefinition.ClassName, 
        typeDefinition.CreateGenerator().Generate()
      );
    }

  }
}
