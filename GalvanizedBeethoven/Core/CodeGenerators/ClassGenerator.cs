using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Constructor;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Events;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Methods;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven
{
  internal class ClassGenerator : ICodeGenerator
  {
    internal static readonly string GeneratedClassName = typeof(IGeneratedClass).FullName;
    private readonly string classNamespace;
    private readonly ConstructorGenerator constructorGenerator;
    private readonly FieldsGenerator fieldsGenerator;
    private readonly PropertiesGenerator propertiesGenerator;
    private readonly MethodsGenerator methodsGenerator;
    private readonly EventsGenerator eventsGenerator;
    private readonly string className;
    private readonly MemberInfo[] membersInfos;

    public ClassGenerator(Type interfaceType, string className, IDefinition[] definitions)
    {
      this.className = className;
      membersInfos = interfaceType
        .GetAllTypes()
        .SelectMany(type => type.GetMembers())
        .Where(FilterMemberInfo)
        .ToArray();
      classNamespace = interfaceType.Namespace;
      constructorGenerator = new ConstructorGenerator(className, definitions);
      fieldsGenerator = new FieldsGenerator(definitions);
      propertiesGenerator = new PropertiesGenerator(membersInfos, definitions);
      methodsGenerator = new MethodsGenerator(membersInfos, definitions);
      eventsGenerator = new EventsGenerator(membersInfos, definitions);
    }

    private static bool FilterMemberInfo(MemberInfo memberInfo) => memberInfo switch
    {
      MethodInfo methodInfo => !methodInfo.IsSpecialName,
      _ => true,
    };

    public IEnumerable<string> Generate(GeneratorContext generatorContext)
    {
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
  }
}