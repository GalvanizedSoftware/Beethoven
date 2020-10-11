using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Constructor;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven
{
  internal class ClassGenerator
  {
    internal static readonly string GeneratedClassName = typeof(IGeneratedClass).FullName;
    private readonly string classNamespace;
    private readonly ConstructorGenerator constructorGenerator;
    private readonly FieldsGenerator fieldsGenerator;
    private readonly PropertiesGenerator propertiesGenerator;
    private readonly MethodGenerators methodGenerators;
    private readonly EventGenerators eventGenerators;
    private readonly GeneratorContext generatorContext;
    private readonly Type interfaceType;
    private readonly string className;

    public ClassGenerator(Type interfaceType, string className, string classNamespace, IDefinition[] definitions)
    {
      this.interfaceType = interfaceType;
      this.className = className;
      this.classNamespace = classNamespace;
      MemberInfo[] membersInfos = interfaceType
        .GetAllTypes()
        .SelectMany(type => type.GetMembers())
        .Where(FilterMemberInfo)
        .ToArray();
      constructorGenerator = new ConstructorGenerator(className, definitions);
      fieldsGenerator = new FieldsGenerator(definitions);
      propertiesGenerator = new PropertiesGenerator(membersInfos, definitions);
      methodGenerators = new MethodGenerators(membersInfos, definitions);
      eventGenerators = new EventGenerators(membersInfos, definitions);
      generatorContext = new GeneratorContext(className, interfaceType);
    }

    public IEnumerable<string> Generate()
    {
      yield return $"namespace {classNamespace}";
      yield return "{";
      yield return $"	public class {className} : {generatorContext.InterfaceType.GetFullName()}, {GeneratedClassName}";
      yield return "	{";
      yield return Generate(fieldsGenerator);
      yield return Generate(constructorGenerator);
      yield return Generate(propertiesGenerator);
      yield return Generate(methodGenerators);
      yield return Generate(eventGenerators);
      yield return "	}";
      yield return "}";
    }

    private string Generate(ICodeGenerator codeGenerator) =>
      codeGenerator
        .Generate(generatorContext)
        .SkipNull()
        .Format(2) + Environment.NewLine;

    private string Generate(ICodeGenerators codeGenerators) =>
      new CodeGeneratorsWrapper(codeGenerators)
        .Generate(generatorContext)
        .SkipNull()
        .Format(2) + Environment.NewLine;


    private static bool FilterMemberInfo(MemberInfo memberInfo) => memberInfo switch
    {
      MethodInfo methodInfo => !methodInfo.IsSpecialName,
      _ => true,
    };
  }
}