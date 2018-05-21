using System;

namespace GalvanizedSoftware.Beethoven.Test
{
  public class InvalidSignature
  {
    public int Simple()
    {
      throw new NotImplementedException();
    }

    public string ReturnValue()
    {
      throw new NotImplementedException();
    }

    public double WithParameters(string text1, string text2)
    {
      throw new NotImplementedException();
    }

    public object WithParameters(string text1, string text2, int count)
    {
      throw new NotImplementedException();
    }

    public bool OutAndRef(out string text1, ref string text2, int count)
    {
      throw new NotImplementedException();
    }
  }
}
