using System;

namespace GalvanizedSoftware.Beethoven.Test.FieldTests
{
  public interface ITestEvents
  {
    event Action Simple;
    event Action<double, string> WithParameters;
    event Func<string, bool> WithReturnValue;
  }
}