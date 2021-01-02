using System;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Extensions;
using System.Linq;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Generic.Methods
{
  public class FieldMappedMethods : IDefinitions
  {
    private readonly string fieldName;
    private readonly Type mainType;

    public FieldMappedMethods(string fieldName, Type mainType)
    {
      this.fieldName = fieldName;
      this.mainType = mainType;
    }

    public IEnumerable<IDefinition> GetDefinitions<TInterface>() where TInterface : class =>
      mainType
        .GetAllMethodsAndInherited()
        .Select(methodInfo => new FieldMappedMethod(methodInfo, fieldName));
  }
}