using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GalvanizedSoftware.Beethoven.Core.Fields
{
  public class ImportedFieldDefinition : IEnumerable<IDefinition>, IMainTypeUser
  {
    private readonly string fieldName;
    private readonly IEnumerable<IDefinition> master;
    private Type mainType;

    public ImportedFieldDefinition(string fieldName, FieldDefinition master)
    {
      this.master = master;
      this.fieldName = fieldName;
    }

    public void Set(Type mainType) =>
      this.mainType = mainType;

    public IEnumerator<IDefinition> GetEnumerator() =>
      master
        .Concat(new FieldMappedMethods(fieldName, mainType))
        .Concat(new FieldMappedProperties(fieldName, mainType))
        .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}
