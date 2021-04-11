using System.Reflection;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Extensions;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
  internal class FieldMappedMethodGenerator : ICodeGenerator
  {
    private readonly string fieldName;
    private readonly MethodInfo methodInfo;

    public FieldMappedMethodGenerator(string fieldName, MemberInfo memberInfo)
    {
      this.fieldName = fieldName;
      methodInfo = memberInfo as MethodInfo;
    }

    public IEnumerable<(CodeType, string)?> Generate() =>
	    MethodsCode.EnumerateCode
	    (
		    new MethodSignatureGenerator(methodInfo).GenerateDeclaration(),
		    "=>".Format(1),
		    $"{fieldName}.;".Format(1)
	    );
  }
}