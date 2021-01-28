using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Test.MethodTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.MethodTests
{
  [TestClass]
  public class FallbackMethodTests
  {

    [TestMethod]
    public void TestMethodFallbackMethod1()
    {
      int callCount = 0;
      ITestMethods test = TypeDefinition<ITestMethods>.Create(
          ActionMethod.Create("Simple", () => callCount++).CreateFallback(),
          FuncMethod.Create("ReturnValue", () => 5)
        )
        .CreateNew();
      Assert.AreEqual(5, test.ReturnValue());
      Assert.AreEqual(0, callCount);
      test.Simple();
      Assert.AreEqual(1, callCount);
    }
  }
}
