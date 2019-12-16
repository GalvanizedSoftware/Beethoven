using System.Collections.Generic;
using System.Collections.Specialized;

namespace GalvanizedSoftware.Beethoven.Test.CompositeTests.Interfaces
{
  public interface IPersonCollection : ICollection<IPerson>, INotifyCollectionChanged
  {
  }
}