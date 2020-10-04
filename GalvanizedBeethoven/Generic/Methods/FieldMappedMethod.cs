using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class FieldMappedMethod : IDefinition
  {
    private readonly string fieldName;
    private readonly IMethodMatcher methodMatcher;

    public FieldMappedMethod(MethodInfo methodInfo, string fieldName)
    {
      this.fieldName = fieldName;
      methodMatcher = new MatchMethodInfoExact(methodInfo);
    }

    public int SortOrder => 2;

    public bool CanGenerate(MemberInfo memberInfo) => memberInfo switch
    {
      MethodInfo methodInfo => methodMatcher.IsMatchIgnoreGeneric(methodInfo, methodInfo.Name),
      _ => false,
    };

    public ICodeGenerator GetGenerator(GeneratorContext _) =>
      new FieldMappedMethodGenerator(fieldName);
  }
}