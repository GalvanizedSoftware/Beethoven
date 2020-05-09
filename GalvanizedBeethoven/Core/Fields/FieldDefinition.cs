using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Constructor;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Fields;
using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using static GalvanizedSoftware.Beethoven.Core.Fields.GeneratorWrapperDefinition;

namespace GalvanizedSoftware.Beethoven.Core.Fields
{
  public class FieldDefinition : IEnumerable<IDefinition>
  {
    private readonly Type type;
    private readonly string fieldName;
    private readonly IEnumerable<IDefinition> definitions;

    public static FieldDefinition CreateFromConstructorParameter<T>() =>
      InternalCreate<T>(fieldDefinition => fieldDefinition.GetConstructorParameterDefinitions<T>());

    public static FieldDefinition CreateFromFactory<T>(Func<T> factoryFunc) =>
      InternalCreate<T>(fieldDefinition => fieldDefinition.GetFactoryDefinitions(factoryFunc));

    private static FieldDefinition InternalCreate<T>(Func<FieldDefinition, IEnumerable<IDefinition>> definitionFactoryFunc)
    {
      Type type = typeof(T);
      string fieldName = $"field{type.GetFormattedName()}{new TagGenerator()}";
      return new FieldDefinition(type, fieldName, definitionFactoryFunc);
    }

    private FieldDefinition(Type type, string fieldName, 
        Func<FieldDefinition, IEnumerable<IDefinition>> definitionFactoryFunc)
    {
      this.type = type;
      this.fieldName = fieldName;
      definitions = definitionFactoryFunc(this).ToArray();
    }

    public ImportedFieldDefinition ImportInMain() =>
      new ImportedFieldDefinition(fieldName, this);

    public IEnumerator<IDefinition> GetEnumerator() => definitions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<IDefinition> GetConstructorParameterDefinitions<T>()
    {
      yield return Create(new SimpleFieldGenerator(type, fieldName));
      yield return Create(new ParameterFieldGenerator(type, fieldName));
    }

    public IEnumerable<IDefinition> GetFactoryDefinitions<T>(Func<T> factoryFunc)
    {
      yield return Create(new FactoryFieldGenerator(type, fieldName, ()=> factoryFunc()));
    }
  }
}
