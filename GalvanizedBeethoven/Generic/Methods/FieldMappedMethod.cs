using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class FieldMappedMethod : DefaultDefinition
  {
    private readonly string fieldName;
    private readonly IMethodMatcher methodMatcher;

    public FieldMappedMethod(MethodInfo methodInfo, string fieldName)
    {
      this.fieldName = fieldName;
      methodMatcher = new MatchMethodInfoExact(methodInfo);
    }

    public override int SortOrder => 2;

    public override bool CanGenerate(MemberInfo memberInfo) =>
      methodMatcher.IsMatchIgnoreGeneric(memberInfo as MethodInfo, memberInfo?.Name);

    public override ICodeGenerator GetGenerator(MemberInfo memberInfo) =>
      new FieldMappedMethodGenerator(fieldName, memberInfo);
  }
}
