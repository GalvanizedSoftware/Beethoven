using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Constructor;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Interfaces;
using static GalvanizedSoftware.Beethoven.Core.Definitions.ConstructorFieldsWrapperDefinition;

namespace GalvanizedSoftware.Beethoven.Generic.Fields
{
  public class FieldDefinition : IDefinitions
  {
    private readonly Type type;
    private readonly string fieldName;
    private readonly IEnumerable<IDefinition> definitions;

    public static FieldDefinition CreateFromConstructorParameter<T>() =>
      InternalCreate<T>(fieldDefinition => fieldDefinition.GetConstructorParameterDefinitions());

    public static FieldDefinition CreateFromFactory<T>(Func<T> factoryFunc) =>
      InternalCreate(fieldDefinition => fieldDefinition.GetFactoryDefinitions(), factoryFunc);

    private static FieldDefinition InternalCreate<T>(Func<FieldDefinition,
      IEnumerable<IDefinition>> definitionFactoryFunc, Func<T> factoryFunc = null)
    {
      Type type = typeof(T);
      string formattedName = type.GetFormattedName();
      string fieldName = $"field{formattedName}{new TagGenerator(formattedName)}";
      string invokerName = $"invoker{formattedName}";

      Func<object> factoryFuncLocal = factoryFunc == null ?
        () => null :
        () => factoryFunc();
      return new(type, fieldName, definitionFactoryFunc, invokerName, factoryFuncLocal);
    }

    private FieldDefinition(Type type, string fieldName,
        Func<FieldDefinition, IEnumerable<IDefinition>> definitionFactoryFunc,
        string invokerName, Func<object> factoryFunc)
    {
      this.type = type;
      this.fieldName = fieldName;
      definitions = definitionFactoryFunc(this)
        .Append(new FieldInstanceFactory(fieldName, factoryFunc))
        .ToArray();
    }

    public IDefinitions ImportInMain() =>
      new ImportedFieldDefinition(this, fieldName);

    public IEnumerable<IDefinition> GetConstructorParameterDefinitions()
    {
      yield return Create(_ => new FieldDeclarationGenerator(type, fieldName));
      yield return Create(_ => new ParameterFieldGenerator(type, fieldName));
    }

    public IEnumerable<IDefinition> GetFactoryDefinitions()
    {
      yield return Create(_ => new FieldDeclarationGenerator(type, fieldName));
      yield return Create(_ => new FactoryFieldGenerator(type, fieldName));
    }

    public IEnumerable<IDefinition> GetDefinitions<T>() where T : class => definitions;
  }
}
