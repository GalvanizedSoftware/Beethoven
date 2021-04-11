using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields
{
  internal class FieldDeclarationGenerator : ICodeGenerator
  {
    private readonly Type type;
    private readonly string fieldName;

    public FieldDeclarationGenerator(Type type, string fieldName)
    {
      this.type = type ?? throw new NullReferenceException();
      this.fieldName = fieldName ?? throw new NullReferenceException();
    }

    public IEnumerable<(CodeType, string)?> Generate() => 
	    FieldsCode.EnumerateCode(
		    $"private {type.GetFullName()} {fieldName};");
  }
}
