using GalvanizedSoftware.Beethoven.Extensions;
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

    internal void SetMainTypeUser<T>() =>
      partDefinitions.OfType<IMainTypeUser>().SetAll(typeof(T));

    internal object[] GetAll<T>() where T : class =>
      partDefinitions
        .Concat(
          new WrapperGenerator<T>(partDefinitions)
          .GetDefinitions())
          .ToArray();
  }
}
