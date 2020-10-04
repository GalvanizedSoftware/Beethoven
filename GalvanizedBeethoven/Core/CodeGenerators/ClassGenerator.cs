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
    private readonly MethodsGenerator methodsGenerator;
    private readonly EventsGenerator eventsGenerator;
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
      methodsGenerator = new MethodsGenerator(membersInfos, definitions);
      eventsGenerator = new EventsGenerator(membersInfos, definitions);
    }

    public IEnumerable<string> Generate()
    {
      GeneratorContext generatorContext = new GeneratorContext(className, interfaceType);
      yield return $"namespace {classNamespace}";
      yield return "{";
      yield return $"	public class {className} : {generatorContext.InterfaceType.GetFullName()}, {GeneratedClassName}";
      yield return "	{";
      yield return $"{fieldsGenerator.Generate(generatorContext).Format(2)}";
      yield return "";
      yield return $"{constructorGenerator.Generate(generatorContext).Format(2)}";
      yield return "";
      yield return $"{propertiesGenerator.Generate(generatorContext).Format(2)}";
      yield return "";
      yield return $"{methodsGenerator.Generate(generatorContext).Format(2)}";
      yield return "";
      yield return $"{eventsGenerator.Generate(generatorContext).Format(2)}";
      yield return "	}";
      yield return "}";
    }

    private static bool FilterMemberInfo(MemberInfo memberInfo) => memberInfo switch
    {
      MethodInfo methodInfo => !methodInfo.IsSpecialName,
      _ => true,
    };
  }
}