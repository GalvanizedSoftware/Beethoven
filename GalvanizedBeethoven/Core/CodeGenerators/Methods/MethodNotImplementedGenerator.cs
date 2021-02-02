using System.Collections.Generic;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Extensions;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
	public sealed class MethodNotImplementedGenerator : ICodeGenerator
	{
		private readonly MethodInfo methodInfo;

		public MethodNotImplementedGenerator(MethodInfo methodInfo)
		{
			this.methodInfo = methodInfo;
		}

		public IEnumerable<(CodeType, string)?> Generate() =>
			MethodsCode.EnumerateCode
			(
				new MethodSignatureGenerator(methodInfo).GenerateDeclaration(),
				"=>".Format(1),
				"throw new System.MissingMethodException();".Format(1),
				""
			);
	}
}