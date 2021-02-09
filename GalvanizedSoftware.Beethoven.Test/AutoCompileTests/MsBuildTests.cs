using DefinitionLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.AutoCompileTests
{
  [TestClass]
  public class MsBuildTests
  {
    [TestMethod]
    public void MsBuildTest1()
    {
      IApproverChain approverChain = new ApproverChain();
      string result = approverChain.Approve(5);
      Assert.AreEqual("Myself", result);
    }

    [TestMethod]
    public void MsBuildTest2()
    {
	    IApproverChain approverChain = new ApproverChain();
	    string result = approverChain.Approve(15);
	    Assert.AreEqual("Local manager", result);
    }

    [TestMethod]
    public void MsBuildTest3()
    {
	    IApproverChain approverChain = new ApproverChain();
	    string result = approverChain.Approve(15000);
	    Assert.IsNull(result);
    }
  }
}
