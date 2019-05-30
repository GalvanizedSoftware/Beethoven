using System;

namespace GalvanizedSoftware.Beethoven.Generic.Parameters
{
  public interface IParameter : IEquatable<IParameter>, IEquatable<(Type, string)>
  {
    Type Type { get; }
  }
}