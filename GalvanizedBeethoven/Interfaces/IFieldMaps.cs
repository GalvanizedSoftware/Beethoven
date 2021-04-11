using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Interfaces
{
  public interface IFieldMaps
  {
    IEnumerable<(string,object)> GetFields();
  }
}