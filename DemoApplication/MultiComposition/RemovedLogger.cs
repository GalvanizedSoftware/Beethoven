using System;

namespace GalvanizedSoftware.Beethoven.DemoApp.MultiComposition
{
  internal class RemovedLogger
  {
    private readonly Func<IPerson, int> getIndex;

    public RemovedLogger(Func<IPerson, int> getIndex)
    {
      this.getIndex = getIndex;
    }

    public int LastIndex { get; set; }

    public bool Remove(IPerson item)
    {
      LastIndex = getIndex(item);
      return LastIndex != -1;
    }
  }
}