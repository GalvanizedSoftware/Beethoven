using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Test.VariousTests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalvanizedSoftware.Beethoven.Test.VariousTests
{
  [TestClass]
  public class ComparerTests
  {
    private readonly MethodInfo methodInfoParameter = typeof(IGenericMethods)
      .GetMethod(nameof(IGenericMethods.Parameter));
    private readonly MethodInfo methodInfoParameter2 = typeof(IGenericMethods2)
      .GetMethod(nameof(IGenericMethods2.Parameter));
    private readonly MethodInfo methodInfoTwoParameters1 = typeof(IGenericMethods)
      .GetMethod(nameof(IGenericMethods.TwoParameters1));
    private readonly MethodInfo methodInfoTwoParameters2 = typeof(IGenericMethods)
      .GetMethod(nameof(IGenericMethods.TwoParameters2));

    [TestMethod]
    public void EquivalentTypeComparerTest1()
    {
      EquivalentTypeComparer equivalentTypeComparer = new EquivalentTypeComparer();
      Assert.IsTrue(equivalentTypeComparer.Equals(
        methodInfoParameter.ReturnType,
        methodInfoParameter.GetParameterTypes().First()));
    }

    [TestMethod]
    public void EquivalentTypeComparerTest2()
    {
      EquivalentTypeComparer equivalentTypeComparer = new EquivalentTypeComparer();
      Assert.AreEqual(
        equivalentTypeComparer.GetHashCode(methodInfoParameter.ReturnType),
        equivalentTypeComparer.GetHashCode(methodInfoParameter.GetParameterTypes().First()));
    }

    [TestMethod]
    public void EquivalentTypeComparerTest3()
    {
      EquivalentTypeComparer equivalentTypeComparer = new EquivalentTypeComparer();
      Assert.IsFalse(equivalentTypeComparer.Equals(typeof(string), typeof(object)));
    }

    [TestMethod]
    public void EquivalentTypeComparerTest4()
    {
      EquivalentTypeComparer equivalentTypeComparer = new EquivalentTypeComparer();
      Assert.AreNotEqual(
        equivalentTypeComparer.GetHashCode(typeof(string)),
        equivalentTypeComparer.GetHashCode(typeof(object)));
    }

    [TestMethod]
    public void EquivalentMethodComparerTest1()
    {
      EquivalentMethodComparer equivalentMethodComparer = new EquivalentMethodComparer();
      Assert.IsTrue(equivalentMethodComparer.Equals(
        methodInfoParameter,
        methodInfoParameter2));
    }

    [TestMethod]
    public void EquivalentMethodComparerTest2()
    {
      EquivalentMethodComparer equivalentMethodComparer = new EquivalentMethodComparer();
      Assert.AreEqual(
        equivalentMethodComparer.GetHashCode(methodInfoParameter),
        equivalentMethodComparer.GetHashCode(methodInfoParameter2));
    }

    [TestMethod]
    public void EquivalentMethodComparerTest3()
    {
      EquivalentMethodComparer equivalentMethodComparer = new EquivalentMethodComparer();
      Assert.IsFalse(equivalentMethodComparer.Equals(methodInfoTwoParameters1, methodInfoParameter));
    }

    [TestMethod]
    public void EquivalentMethodComparerTest4()
    {
      EquivalentMethodComparer equivalentMethodComparer = new EquivalentMethodComparer();
      Assert.AreNotEqual(
        equivalentMethodComparer.GetHashCode(methodInfoTwoParameters1),
        equivalentMethodComparer.GetHashCode(methodInfoParameter));
    }

    [TestMethod]
    public void EquivalentMethodComparerTest5()
    {
      EquivalentMethodComparer equivalentMethodComparer = new EquivalentMethodComparer();
      Assert.IsFalse(equivalentMethodComparer.Equals(methodInfoTwoParameters1, methodInfoTwoParameters2));
    }

    [TestMethod]
    public void EquivalentMethodComparerTest6()
    {
      EquivalentMethodComparer equivalentMethodComparer = new EquivalentMethodComparer();
      Assert.AreNotEqual(
        equivalentMethodComparer.GetHashCode(methodInfoTwoParameters1),
        equivalentMethodComparer.GetHashCode(methodInfoTwoParameters2));
    }
  }
}
