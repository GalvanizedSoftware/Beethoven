using System;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Core;
using System.Linq;
using System.Collections;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class FieldMappedMethods : IEnumerable<IDefinition>
  {
    private readonly string fieldName;
    private Type mainType;

    public FieldMappedMethods(string fieldName, Type mainType)
    {
      this.fieldName = fieldName;
      this.mainType = mainType;
    }

    public IEnumerator<IDefinition> GetEnumerator() =>
      mainType
        .GetAllMethodsAndInherited()
        .Select(methodInfo => new FieldMappedMethod(methodInfo, fieldName))
        .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}