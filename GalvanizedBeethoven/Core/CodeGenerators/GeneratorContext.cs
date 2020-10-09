using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using static System.Environment;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators
{
  public class GeneratorContext
  {
    private static readonly ConstructorInfo dummyConstructorInfo = typeof(object).GetConstructor(Array.Empty<Type>());
    private static readonly FieldInfo dummyFieldInfo = typeof(int).GetField(nameof(int.MaxValue));

    Dictionary<CodeType, MemberInfo> codeTypeMapping = new Dictionary<CodeType, MemberInfo>
    {
      { CodeType.ConstructorCode,  dummyConstructorInfo},
      { CodeType.ConstructorSignature,  dummyConstructorInfo},
      { CodeType.Fields,  dummyFieldInfo},
    };
    Dictionary<Type, CodeType> typeCodeTypeMapping = new Dictionary<Type, CodeType>
    {
      { typeof(FieldInfo), CodeType.Fields},
      { typeof(ConstructorInfo), CodeType.ConstructorCode},
      { typeof(MethodInfo), CodeType.Methods},
      { typeof(PropertyInfo), CodeType.Properties},
      { typeof(EventInfo), CodeType.Events}
    };

    internal GeneratorContext(string generatedClassName, Type interfaceType)
    {
      GeneratedClassName = generatedClassName;
      InterfaceType = interfaceType;
    }

    private GeneratorContext(GeneratorContext baseContext, MemberInfo memberInfo, CodeType codeType, int? methodIndex = null) :
      this(baseContext.GeneratedClassName, baseContext.InterfaceType)
    {
      MemberInfo = memberInfo;
      CodeType = codeType;
      MethodIndex = methodIndex;
    }

    public string GeneratedClassName { get; }
    public Type InterfaceType { get; }
    public MemberInfo MemberInfo { get; }

    internal CodeType CodeType { get; }

    public int? MethodIndex { get; }

    internal GeneratorContext CreateLocal(CodeType codeType, MemberInfo memberInfo = null, int? methodIndex = null) =>
      new GeneratorContext(
        this,
        memberInfo ?? codeTypeMapping[codeType],
        codeType,
        methodIndex);

    internal GeneratorContext CreateLocal<T>(T memberInfo, int? methodIndex = null) where T : MemberInfo =>
      new GeneratorContext(
        this,
        memberInfo,
        typeCodeTypeMapping[typeof(T)],
        methodIndex);

    public override int GetHashCode() =>
      ($"{GeneratedClassName}" + NewLine +
      $"{InterfaceType.FullName}" + NewLine +
      $"{MemberInfo.Name}" + NewLine +
      $"{(MethodIndex?.ToString(CultureInfo.InvariantCulture) ?? "")}").GetHashCode();
  }
}