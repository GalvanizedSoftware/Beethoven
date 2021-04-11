using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.Invokers.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods
{
	internal class MethodGenerator : ICodeGenerator
	{
		private readonly MethodInfo methodInfo;
		private readonly string invokerName;

		public MethodGenerator(MethodInfo methodInfo, int methodIndex)
		{
			this.methodInfo = methodInfo;
			string methodName = $"{methodInfo?.Name}{methodIndex}";
			invokerName = $"invoker{methodName}";
		}

		public IEnumerable<(CodeType, string)?> Generate()
		{
			CodeGeneratorList invokerGenerators = new(
				new FieldDeclarationGenerator(typeof(MethodInvokerInstance), invokerName),
				new MethodInvokerGenerator(invokerName));
			return invokerGenerators.Generate()
				.Concat(MethodsCode.EnumerateCode(GenerateLocal()));
		}

		private IEnumerable<string> GenerateLocal()
		{
			ParameterInfo[] parameters = methodInfo.GetParametersSafe().ToArray();
			Type returnType = methodInfo?.ReturnType;
			MethodSignatureGenerator methodSignatureGenerator = new(methodInfo);
			foreach (string line in methodSignatureGenerator.GenerateDeclaration())
				yield return line;
			yield return "{";
			string parametersName = $"{invokerName}Parameters";
			ParametersGenerator parametersGenerator = new(parameters, parametersName);
			yield return parametersGenerator.GeneratePreInvoke().Format(1);
			string returnValueName = $"{invokerName}Result";
			yield return $"object {returnValueName} = {invokerName}.Invoke({GetGenericTypes()}, {parametersName});".Format(1);
			foreach (string line in parametersGenerator.GeneratePostInvoke())
				yield return line.Format(1);
			if (returnType != typeof(void))
				yield return $"return ({returnType.GetFullName()}){returnValueName};".Format(1);
			yield return "}";
			yield return "";
		}

		private string GetGenericTypes() =>
			"new System.Type[]" +
			$"{{{string.Join(", ", methodInfo.GetGenericArguments().Select(GetTypeof))}}}";

		private static string GetTypeof(Type type) =>
			$"typeof({type.GetFullName()})";
	}
}