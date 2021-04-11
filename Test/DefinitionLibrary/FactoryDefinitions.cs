using System.Collections.Generic;
using GalvanizedSoftware.Beethoven;
using GalvanizedSoftware.Beethoven.Generic.Methods;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace DefinitionLibrary
{
  public class FactoryDefinitions : IFactoryDefinition<IApproverChain>
  {
    [Factory]
    public FactoryDefinitions()
    {
    }

    public string Namespace => "DefinitionLibrary";

    public string ClassName => "ApproverChain";

    public IEnumerable<object> PartDefinitions
    {
      get
      {
        yield return LinkedMethodsReturnValue.Create<IApproverChain>(nameof(IApproverChain.Approve))
          .AutoMappedMethod(new Myself()).InvertResult()
          .AutoMappedMethod(new LocalManager()).InvertResult()
          .AutoMappedMethod(new Level2Manager()).InvertResult()
          .AutoMappedMethod(new Level1Manager()).InvertResult();
      }
    }
  }
}
