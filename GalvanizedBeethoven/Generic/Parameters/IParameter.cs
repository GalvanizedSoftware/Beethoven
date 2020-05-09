using GalvanizedSoftware.Beethoven.Core;
using System;

namespace GalvanizedSoftware.Beethoven.Generic.Parameters
{
  public interface IParameter
  {
    Type Type { get; }
    string Name { get; }
  }
}