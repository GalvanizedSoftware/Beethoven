using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Constructor;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Environment;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven
{
  internal class ClassGenerator
  {
    internal static readonly string GeneratedClassName = typeof(IGeneratedClass).FullName;
    private readonly string classNamespace;
    private readonly IDefinition[] definitions;
    private readonly ConstructorGenerator constructorGenerator;
    private readonly PropertyGenerators propertiesGenerator;
    private readonly MethodGenerators methodGenerators;
    private readonly EventGenerators eventGenerators;
    private readonly GeneratorContext generatorContext;
    private readonly string className;

    public ClassGenerator(Type interfaceType, string className, string classNamespace, IDefinition[] definitions)
    {
      this.className = className;
      this.classNamespace = classNamespace;
      this.definitions = definitions;
      MemberInfo[] membersInfos = interfaceType
        .GetAllTypes()
        .SelectMany(type => type.GetMembers())
        .Where(FilterMemberInfo)
        .ToArray();
      constructorGenerator = new ConstructorGenerator(className);
      propertiesGenerator = new PropertyGenerators(membersInfos, definitions);
      methodGenerators = new MethodGenerators(membersInfos, definitions);
      eventGenerators = new EventGenerators(membersInfos, definitions);

      generatorContext = new GeneratorContext(className, interfaceType);
    }

    public IEnumerable<string> Generate()
    {
      (CodeType, string)[] code = definitions.GenerateCode(generatorContext)
        .Concat(Generate(propertiesGenerator))
        .Concat(Generate(methodGenerators))
        .Concat(Generate(eventGenerators))
        .ToArray();
      yield return $"namespace {classNamespace}";
      yield return "{";
      yield return $"public class {className} : {generatorContext.InterfaceType.GetFullName()}, {GeneratedClassName}".Format(1);
      yield return "{".Format(1);
      yield return Generate(code, FieldsCode);
      yield return GenerateConstructorCode(code);
      yield return Generate(code, PropertiesCode);
      yield return Generate(code, MethodsCode);
      yield return Generate(code, EventsCode);
      yield return "	}";
      yield return "}";
    }

    private static string Generate((CodeType, string)[] code, CodeType filter) =>
      code
        .Filter(filter)
        .Format(2) + NewLine;

    private string GenerateConstructorCode((CodeType, string)[] code) =>
      constructorGenerator
        .Generate(code)
        .Format(2) + NewLine;

    private IEnumerable<(CodeType, string)> Generate(ICodeGenerators codeGenerators) =>
      codeGenerators
        .GetGenerators(generatorContext)
        .SelectMany(codeGenerator => codeGenerator.Generate())
        .SkipNull();


    private static bool FilterMemberInfo(MemberInfo memberInfo) => memberInfo switch
    {
      MethodInfo methodInfo => !methodInfo.IsSpecialName,
      _ => true,
    };
  }
}