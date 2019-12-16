using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Generic
{
  public class DefinitionImport
  {
    public DefinitionImport(IParameter parameter)
    {
      Parameter = parameter;
    }

    public IParameter Parameter { get; }
  }
}