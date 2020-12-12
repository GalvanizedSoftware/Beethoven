using System;

namespace GalvanizedSoftware.Beethoven.Test.FieldTests.Interfaces
{
  public interface ITestEvents
  {
    event Action Simple;
    event Action<double, string> WithParameters;
    event Func<string, bool> WithReturnValue;
  }
}