using System.ComponentModel;

namespace GalvanizedSoftware.Beethoven.Test.VariousTests.Implementations
{
  internal class FullImplements
  {
    public int Property1 { get; set; }
    public string Property2 { get; set; }

    public int Method1(int a, object b)
    {
      return 0;
    }

    public int Method2(ref string c)
    {
      return 0;
    }

    public void Method3()
    {
    }

#pragma warning disable 67
    public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67
  }
}
