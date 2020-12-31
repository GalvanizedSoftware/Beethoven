using System;
using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
  class PartDefinitions
  {
    private readonly object[] partDefinitions;

    public PartDefinitions(IEnumerable<object> newPartDefinitions)
    {
      partDefinitions = newPartDefinitions.ToArray();
    }

    internal PartDefinitions Concat(object[] concatPartDefinitions) =>
      new(partDefinitions.Concat(concatPartDefinitions));

    internal void SetMainTypeUser(Type mainType) =>
      partDefinitions.OfType<IMainTypeUser>().SetAll(mainType);

    internal object[] GetAll<T>() where T : class =>
      partDefinitions
        .Concat(
          new WrapperGenerator<T>(partDefinitions)
          .GetDefinitions())
          .ToArray();
  }
}
