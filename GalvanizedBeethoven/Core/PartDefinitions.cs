using GalvanizedSoftware.Beethoven.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core
{
  class PartDefinitions
  {
    private object[] partDefinitions;

    public PartDefinitions(object[] newPartDefinitions)
    {
      partDefinitions = newPartDefinitions;
    }

    internal PartDefinitions Concat(object[] concatPartDefinitions) =>
      new PartDefinitions(partDefinitions.Concat(concatPartDefinitions).ToArray());

    internal void SetMainTypeUser(Type mainType) =>
      partDefinitions.OfType<IMainTypeUser>().SetAll(mainType);

    internal object[] GetAll<T>() where T : class =>
      partDefinitions
        .Concat(
          new WrapperGenerator<T>(partDefinitions)
          .GetDefinitions())
          .ToArray();

    public IEnumerable<IDefinition> GetDefinitions() =>
    //partDefinitions.GetAllDefinitions();
      partDefinitions.OfType<IDefinition>();
  }
}
