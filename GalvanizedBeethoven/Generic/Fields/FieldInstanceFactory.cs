using System;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Fields
{
  public class FieldInstanceFactory : DefaultDefinition
  {
    private readonly string fieldName;
    private readonly Func<object> factoryFunc;

    public FieldInstanceFactory(string fieldName, Func<object> factoryFunc)
    {
      this.fieldName = fieldName;
      this.factoryFunc = factoryFunc;
    }

    public override ICodeGenerator GetGenerator(GeneratorContext generatorContext) =>
      null;

    public override IEnumerable<(string, object)> GetFields()
    {
      yield return (fieldName, factoryFunc());
    }
  }
}
