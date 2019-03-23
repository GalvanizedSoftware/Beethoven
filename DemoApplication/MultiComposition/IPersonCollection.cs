using System.Collections.Generic;
using System.Collections.Specialized;

namespace GalvanizedSoftware.Beethoven.DemoApp.MultiComposition
{
  public interface IPersonCollection : ICollection<IPerson>, INotifyCollectionChanged
  {
  }
}