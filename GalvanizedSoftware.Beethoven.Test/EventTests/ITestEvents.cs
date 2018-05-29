using System;

namespace GalvanizedSoftware.Beethoven.Test.EventTests
{
  public interface ITestEvents
  {
    event Action Simple;
    event Action<double, string> WithParameters;
    event Func<string, bool> WithReturnValue;
  }
}