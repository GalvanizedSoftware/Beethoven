using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Methods.MethodMatchers;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
	public class NotImplementedMethod : MethodDefinition
	{
		private readonly MethodInfo methodInfo;

		public NotImplementedMethod(MethodInfo methodInfo) :
			base(methodInfo.Name, new MatchAnything())
		{
			this.methodInfo = methodInfo;
		}

		public override int SortOrder => 2;

		public override ICodeGenerator GetGenerator(GeneratorContext generatorContext) => 
			new MethodNotImplementedGenerator(methodInfo);
	}
}
