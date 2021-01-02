using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Interfaces;
using static GalvanizedSoftware.Beethoven.Core.ReflectionConstants;

namespace GalvanizedSoftware.Beethoven.Build
{
  internal class CodeGenerator
  {
    private static readonly MethodInfo generateCodeInternalMethod = typeof(CodeGenerator)
      .GetMethods(ResolveFlags)
      .Where(info => info.Name == nameof(GenerateCode))
      .FirstOrDefault(info => info.IsGenericMethod);
   private readonly object factory;
    private readonly MethodInfo generateMethod;
    private readonly MemberInfo factoryMemberInfo;

    internal static CodeGenerator Create(Type type, object factory, MemberInfo factoryMemberInfo) =>
      new CodeGenerator(factory, generateCodeInternalMethod.MakeGenericMethod(type), factoryMemberInfo);
    private CodeGenerator(object factory, MethodInfo generateMethod, MemberInfo factoryMemberInfo)
    {
      this.factory = factory;
      this.generateMethod = generateMethod;
      this.factoryMemberInfo = factoryMemberInfo;
    }

    public (string filename, string code) GenerateCode() =>
      ((string filename, string code))generateMethod.Invoke(this, new object[0]);

    private (string filename, string code) GenerateCode<T>() where T : class
    {
      IFactoryDefinition<T> factoryDefinition = factory as IFactoryDefinition<T>;
      TypeDefinition<T> typeDefinition = TypeDefinition<T>
        .CreateFromFactoryDefinition(factoryDefinition, factoryMemberInfo);
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
