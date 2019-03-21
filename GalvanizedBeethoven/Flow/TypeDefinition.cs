using System.Linq;

namespace GalvanizedSoftware.Beethoven.Flow
{
  public class TypeDefinition<T> where T : class
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();
    private readonly object[] implementationObjects;

    public TypeDefinition(params object[] newImplementationObjects)
    {
      implementationObjects = newImplementationObjects;
    }

    private TypeDefinition(TypeDefinition<T> previousDefinition, object[] newImplementationObjects)
    {
      beethovenFactory = previousDefinition.beethovenFactory;
      implementationObjects = previousDefinition.implementationObjects.Concat(newImplementationObjects).ToArray();
    }

    public TypeDefinition<T> Add(params object[] newImplementationObjects) =>
      new TypeDefinition<T>(this, newImplementationObjects);

    public T Create() => beethovenFactory.Generate<T>(implementationObjects);
  }
}
