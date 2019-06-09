using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces
{
  internal class MultiImplementationInt
  {
    public List<int> CallList { get; } = new List<int>();

    void Foo(int a) => 
      CallList.Add(a);
  }
}
