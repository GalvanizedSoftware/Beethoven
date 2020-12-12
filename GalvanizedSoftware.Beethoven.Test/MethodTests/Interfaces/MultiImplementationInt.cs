using System.Collections.Generic;
// ReSharper disable UnusedMember.Global

namespace GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces
{
  internal class MultiImplementationInt
  {
    public List<int> CallList { get; } = new List<int>();

    public void Foo(int a) => 
      CallList.Add(a);
  }
}
