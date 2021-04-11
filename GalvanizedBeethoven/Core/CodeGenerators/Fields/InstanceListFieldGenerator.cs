using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.FieldInstances;
using GalvanizedSoftware.Beethoven.Extensions;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;
using static GalvanizedSoftware.Beethoven.Core.GeneratorHelper;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
  internal class InstanceListFieldGenerator<T> : ICodeGenerator where T : class
  {
	  private const string CreateName = nameof(InstanceList<T>.Create);
	  private static readonly string instanceListType = typeof(InstanceList<T>).GetFullName();

    public static InstanceListFieldGenerator<T> Create(MemberInfo factoryMemberInfo) =>
      new(GetFactory(factoryMemberInfo));

    public static InstanceListFieldGenerator<T> Create(string id) =>
      new(GetFromId(id));

    private readonly string factoryCode;

    private InstanceListFieldGenerator(string factoryCode)
    {
      this.factoryCode = factoryCode;
    }

    public IEnumerable<(CodeType, string)?> Generate() =>
	    FieldsCode.EnumerateCode(
		    $@"private {instanceListType} {InstanceListName} = {instanceListType}.{CreateName}({factoryCode});");

    private static string GetFactory(MemberInfo factoryMemberInfo) =>
      factoryMemberInfo switch
      {
        ConstructorInfo constructorInfo => GetFromConstructor(constructorInfo),
        MethodInfo memberInfo => GetFromMethod(memberInfo),
        _ => ""
      };

    private static string GetFromMethod(MethodInfo memberInfo) =>
      $"{memberInfo.DeclaringType.GetFullName()}.{memberInfo.Name}()";

    private static string GetFromConstructor(ConstructorInfo constructorInfo) =>
      $"new {constructorInfo.DeclaringType.GetFullName()}()";

    private static string GetFromId(string id) =>
      @$"""{id}""";
  }
}