using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Generic;

namespace GalvanizedSoftware.Beethoven.Core.Methods
{
  public class GenericMethodWrapper : DefaultDefinition
  {

    public GenericMethodWrapper(MethodInfo methodInfo, MethodDefinition methodDefinition)
    {
      MethodInfo = methodInfo;
      MethodDefinition = methodDefinition;
    }

    public override bool CanGenerate(MemberInfo memberInfo) => 
      false;

    public MethodInfo MethodInfo { get; }

    public MethodDefinition MethodDefinition { get; }

    public override ICodeGenerator GetGenerator(GeneratorContext generatorContext) =>
      null;
  }
}