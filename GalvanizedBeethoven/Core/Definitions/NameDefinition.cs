namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
  public class NameDefinition
  {
    public NameDefinition(string className, string classNamespace)
    {
      ClassName = className;
      ClassNamespace = classNamespace;
    }

    public string ClassName { get; }
    public string ClassNamespace { get; }
  }
}
