using GalvanizedSoftware.Beethoven.Generic.Properties;
using GalvanizedSoftware.Beethoven.Generic.ValueLookup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Unity;

namespace GalvanizedSoftware.Beethoven.Test.CompositeTests
{
  [TestClass]
  public class AutoAssignTests
  {
    [TestMethod]
    public void AutoAssignTest1()
    {
      Dictionary<string, object> defaultValues = new Dictionary<string, object>
      {
        { "Name", "The evil company"},
        { "Address","2460 Sunshine road"}
      };
      BeethovenFactory factory = new BeethovenFactory();
      IValueLookup lookup = new CompositeValueLookup(
        new DictionaryValueLookup(defaultValues),
        new InterfaceFactoryValueLookup((type, name) => factory.Generate(type)));
      factory.GeneralPartDefinitions = new object[]
      {
        new DefaultProperty()
        .ValueLookup(lookup)
        .SetterGetter()
      };
      ICompany company = factory.Generate<ICompany>();
      Assert.AreEqual("The evil company", company.Information.Name);
      Assert.AreEqual("2460 Sunshine road", company.Information.Address);
    }

    [TestMethod]
    public void AutoAssignTest2()
    {
      var defaultValues = new
      {
        Name = "The evil company",
        Address = "2460 Sunshine road"
      };
      BeethovenFactory factory = new BeethovenFactory();
      IValueLookup lookup = new CompositeValueLookup(
        new AnonymousValueLookup(defaultValues),
        new InterfaceFactoryValueLookup((type, name) => factory.Generate(type)));
      factory.GeneralPartDefinitions = new object[]
      {
        new DefaultProperty()
          .ValueLookup(lookup)
          .SetterGetter()
      };
      ICompany company = factory.Generate<ICompany>();
      Assert.AreEqual("The evil company", company.Information.Name);
      Assert.AreEqual("2460 Sunshine road", company.Information.Address);
    }

    [TestMethod]
    public void AutoAssignTest3()
    {
      var defaultValues = new
      {
        Name = "The evil company",
        Address = "2460 Sunshine road"
      };
      BeethovenFactory factory = new BeethovenFactory();
      factory.GeneralPartDefinitions = new object[]
      {
        new DefaultProperty()
          .AnonymousValueLookup(defaultValues)
          .SetterGetter()
      };
      ICompanyInformation companyInformation = factory.Generate<ICompanyInformation>();
      Assert.AreEqual("The evil company", companyInformation.Name);
      Assert.AreEqual("2460 Sunshine road", companyInformation.Address);
    }

    [TestMethod]
    public void AutoAssignTestIoc()
    {
      UnityContainer unityContainer = new UnityContainer();
      unityContainer.RegisterInstance("informationName", "The evil company");
      unityContainer.RegisterInstance("informationAddress", "2460 Sunshine road");
      unityContainer.Resolve<IocFactory>();
      ICompany company = unityContainer.Resolve<ICompany>();
      Assert.AreEqual("The evil company", company.Information.Name);
      Assert.AreEqual("2460 Sunshine road", company.Information.Address);
    }
  }
}
