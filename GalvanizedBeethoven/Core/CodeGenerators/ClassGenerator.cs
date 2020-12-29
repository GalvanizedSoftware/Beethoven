using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Constructor;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using static System.Environment;
using static GalvanizedSoftware.Beethoven.Core.CodeGenerators.CodeType;

namespace GalvanizedSoftware.Beethoven.Core.CodeGenerators
{
  internal class ClassGenerator
  {
    internal static readonly string GeneratedClassName = typeof(IGeneratedClass).FullName;
    private readonly NameDefinition nameDefinition;
    private readonly IDefinition[] definitions;
    private readonly ConstructorGenerator constructorGenerator;
    private readonly PropertyGenerators propertiesGenerator;
    private readonly MethodGenerators methodGenerators;
    private readonly EventGenerators eventGenerators;
    private readonly GeneratorContext generatorContext;

    public ClassGenerator(MemberInfoList memberInfoList, Type interfaceType, NameDefinition nameDefinition, IDefinition[] definitions)
    {
      this.nameDefinition = nameDefinition;
      this.definitions = definitions;
      constructorGenerator = new ConstructorGenerator(nameDefinition.ClassName);
      propertiesGenerator = new PropertyGenerators(memberInfoList.PropertyInfos, definitions);
      methodGenerators = new MethodGenerators(
        memberInfoList.MethodInfos, memberInfoList.MethodIndexes, definitions);
      eventGenerators = new EventGenerators(memberInfoList.EventInfos, definitions);
      generatorContext = new GeneratorContext(nameDefinition.ClassName, interfaceType);
    }

    public IEnumerable<string> Generate()
    {
      (CodeType, string)[] code = definitions.GenerateCode(generatorContext)
        .Concat(Generate(propertiesGenerator))
        .Concat(Generate(methodGenerators))
        .Concat(Generate(eventGenerators))
        .ToArray();
      yield return $"namespace {nameDefinition.ClassNamespace}";
      yield return "{";
      yield return $"public class {nameDefinition.ClassName} : {generatorContext.InterfaceType.GetFullName()}, {GeneratedClassName}".Format(1);
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
  }
}