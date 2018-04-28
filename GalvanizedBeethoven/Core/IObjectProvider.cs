using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal interface IObjectProvider
  {
    IEnumerable<TChild> Get<TChild>();
  }
}