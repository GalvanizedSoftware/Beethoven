using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Generic.Fields
{
  internal class ImportedFieldDefinition : IDefinitions
  {
    private readonly string fieldName;
    private readonly IDefinitions master;

    internal ImportedFieldDefinition(FieldDefinition master, string fieldName)
    {
      this.master = master;
      this.fieldName = fieldName;
    }

    public IEnumerable<IDefinition> GetDefinitions<T>() where T : class =>
       master
        .GetDefinitions<T>()
        .Concat(new FieldMappedMethods(fieldName, typeof(T)).GetDefinitions<T>())
        .Concat(new FieldMappedProperties(fieldName, typeof(T)).GetDefinitions<T>());
  }
}
