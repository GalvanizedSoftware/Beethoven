using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Generic.Methods;

namespace GalvanizedSoftware.Beethoven.DemoApp.ChainOfResponsibility1
{
  public class FactoryDefinitions : IFactoryDefinitions
  {
    public string Namespace => "GalvanizedSoftware.DefinitionLibrary";

    public string ClassName => "ApproverChain";

    public IEnumerable<object> PartDefinitions
    {
      get
      {
        yield return LinkedMethodsReturnValue.Create<IApproverChain>(nameof(IApproverChain.Approve))
          .AutoMappedMethod(new Myself()).InvertResult()
          .AutoMappedMethod(new LocalManager()).InvertResult()
          .AutoMappedMethod(new Level2Manager()).InvertResult()
          .AutoMappedMethod(new Level1Manager()).InvertResult()
      }
    }
  }
}
