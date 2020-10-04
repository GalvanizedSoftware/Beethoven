using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core
{
  class PartDefinitions
  {
    private object[] partDefinitions;

    public PartDefinitions(IEnumerable<object> newPartDefinitions)
    {
      partDefinitions = newPartDefinitions.ToArray();
    }

    internal PartDefinitions Concat(object[] concatPartDefinitions) =>
      new PartDefinitions(partDefinitions.Concat(concatPartDefinitions));

    internal void SetMainTypeUser(Type mainType) =>
      partDefinitions.OfType<IMainTypeUser>().SetAll(mainType);

    internal object[] GetAll<T>() where T : class =>
      partDefinitions
        .Concat(
          new WrapperGenerator<T>(partDefinitions)
          .GetDefinitions())
          .ToArray();

    public IEnumerable<IDefinition> GetDefinitions() =>
      partDefinitions.OfType<IDefinition>();
  }
}
