using System;

namespace GalvanizedSoftware.Beethoven.Generic.Parameters
{
  public interface IParameter : IComparable<IParameter>
  {
    Type Type { get; }
  }
}