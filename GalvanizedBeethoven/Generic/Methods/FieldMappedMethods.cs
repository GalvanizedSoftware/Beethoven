using System;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Core;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class FieldMappedMethods : IDefinitions
  {
    private readonly string fieldName;
    private Type mainType;

    public FieldMappedMethods(string fieldName, Type mainType)
    {
      this.fieldName = fieldName;
      this.mainType = mainType;
    }

    public IEnumerable<IDefinition> GetDefinitions() =>
      mainType
        .GetAllMethodsAndInherited()
        .Select(methodInfo => new FieldMappedMethod(methodInfo, fieldName));
  }
}