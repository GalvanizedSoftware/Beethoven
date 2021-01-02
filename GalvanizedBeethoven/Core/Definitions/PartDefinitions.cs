using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
  internal class PartDefinitions : IEnumerable<object>
  {
    private readonly object[] partDefinitions;

    public PartDefinitions(IEnumerable<object> newPartDefinitions)
    {
      partDefinitions = newPartDefinitions.ToArray();
    }

    internal PartDefinitions Concat(params object[] concatPartDefinitions) =>
      new(partDefinitions.Concat(concatPartDefinitions));

    public PartDefinitions Set<T>(T definition) where T : class =>
      new(partDefinitions
        .Where(o => o is not T)
        .Append(definition));

    internal void SetMainTypeUser(Type mainType) =>
      partDefinitions.OfType<IMainTypeUser>().SetAll(mainType);

    //internal PartDefinitions GetAll<T>() where T : class
    //{
    //  object[] definitions = partDefinitions
    //    .Concat(
    //      new MappedDefinitions<T>(partDefinitions)
    //        .GetDefinitions<T>())
    //    .GetAllDefinitions<T>()
    //    .ToArray();
    //  SetMainTypeUser(typeof(T));
    //  return new(definitions);
    //}

    internal PartDefinitions GetAll<T>() where T : class =>
      new(partDefinitions
        .Concat(
          new MappedDefinitions<T>(partDefinitions)
          .GetDefinitions<T>()));

    public IEnumerator<object> GetEnumerator() =>
      partDefinitions.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
      GetEnumerator();
  }
}
