﻿using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Generic.Properties;
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

    public void Set(Type mainType) =>
      this.mainType = mainType;

    public IEnumerable<IDefinition> GetDefinitions() =>
       master
        .GetDefinitions()
        .Concat(new FieldMappedMethods(fieldName, mainType).GetDefinitions())
        .Concat(new FieldMappedProperties(fieldName, mainType).GetDefinitions());
  }
}