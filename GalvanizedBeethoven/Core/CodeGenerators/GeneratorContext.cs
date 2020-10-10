using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using static System.Environment;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators
{
  public class GeneratorContext
  {
    Dictionary<Type, CodeType> typeCodeTypeMapping = new Dictionary<Type, CodeType>
    {
      { typeof(MethodInfo), CodeType.Methods},
      { typeof(PropertyInfo), CodeType.Properties},
      { typeof(EventInfo), CodeType.Events}
    };

    internal GeneratorContext(string generatedClassName, Type interfaceType)
    {
      GeneratedClassName = generatedClassName;
      InterfaceType = interfaceType;
    }

    private GeneratorContext(GeneratorContext baseContext, CodeType codeType,
      MemberInfo memberInfo = null, int? methodIndex = null) :
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

    internal GeneratorContext CreateLocal(CodeType codeType) =>
      new GeneratorContext(this, codeType);

    internal GeneratorContext CreateLocal<T>(T memberInfo, int? methodIndex = null) where T : MemberInfo =>
      new GeneratorContext(
        this,
        typeCodeTypeMapping[typeof(T)],
        memberInfo,
        methodIndex);

    public override int GetHashCode() =>
      ($"{GeneratedClassName}" + NewLine +
      $"{InterfaceType.FullName}" + NewLine +
      $"{MemberInfo.Name}" + NewLine +
      $"{(MethodIndex?.ToString(CultureInfo.InvariantCulture) ?? "")}").GetHashCode();
  }
}