﻿using System;
using System.Globalization;
using System.Reflection;
using static System.Environment;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators
{
  public class GeneratorContext
  {
    internal GeneratorContext(string generatedClassName, Type interfaceType)
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

    public string GeneratedClassName { get; }
    public Type InterfaceType { get; }
    public MemberInfo MemberInfo { get; }
    public int? MethodIndex { get; }

    internal GeneratorContext CreateLocal(MemberInfo memberInfo, int? methodIndex = null) =>
      new GeneratorContext(this, memberInfo, methodIndex);

    public override int GetHashCode() =>
      ($"{GeneratedClassName}" + NewLine +
      $"{InterfaceType.FullName}" + NewLine +
      $"{MemberInfo.Name}" + NewLine +
      $"{(MethodIndex?.ToString(CultureInfo.InvariantCulture) ?? "")}").GetHashCode();
  }
}