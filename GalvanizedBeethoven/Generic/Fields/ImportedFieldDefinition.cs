using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Generic.Fields
{
  internal class ImportedFieldDefinition : IDefinitions, IMainTypeUser
  {
    private readonly string fieldName;
    private readonly IDefinitions master;
    private Type mainType;

    internal ImportedFieldDefinition(FieldDefinition master, string fieldName)
    {
      this.master = master;
      this.fieldName = fieldName;
    }

    public void Set(Type setMainType) =>
      mainType = setMainType;

    public IEnumerable<IDefinition> GetDefinitions<TInterface>() where TInterface : class =>
       master
        .GetDefinitions<TInterface>()
        .Concat(new FieldMappedMethods(fieldName, mainType).GetDefinitions<TInterface>())
        .Concat(new FieldMappedProperties(fieldName, mainType).GetDefinitions<TInterface>());
  }
}
