using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators
{
  public class GeneratorContext
  {
    public GeneratorContext(string generatedClassName, Type interfaceType)
    {
      GeneratedClassName = generatedClassName;
      InterfaceType = interfaceType;
    }

    private GeneratorContext(GeneratorContext baseContext, MemberInfo memberInfo, int? methodIndex = null) :
      this(baseContext.GeneratedClassName, baseContext.InterfaceType)
    {
      MemberInfo = memberInfo;
      MethodIndex = methodIndex;
    }

    public GeneratorContext CreateLocal(string memberName, int index = -1) =>
      new GeneratorContext(this, InterfaceType.FindMember(memberName, index));

    public string GeneratedClassName { get; }
    public Type InterfaceType { get; }
    public MemberInfo MemberInfo { get; }

    public int? MethodIndex { get; }

    internal GeneratorContext CreateLocal(MemberInfo memberInfo, int? methodIndex = null) =>
      new GeneratorContext(this, memberInfo, methodIndex);
  }
}