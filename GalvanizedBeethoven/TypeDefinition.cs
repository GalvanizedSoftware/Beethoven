using System;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Methods;

namespace GalvanizedSoftware.Beethoven
{
  public class TypeDefinition<T> where T : class
  {
    private readonly BeethovenFactory beethovenFactory = new BeethovenFactory();
    private readonly object[] partDefinitions;

    public TypeDefinition(params object[] newPartDefinitions)
    {
      partDefinitions = newPartDefinitions;
    }

    private TypeDefinition(TypeDefinition<T> previousDefinition, object[] newPartDefinitions) :
      this(previousDefinition.partDefinitions.Concat(newPartDefinitions).ToArray())
    {
    }

    public CompiledTypeDefinition<T> Compile() =>
      beethovenFactory.Compile<T>(partDefinitions);

    public TypeDefinition<T> Add(params object[] newImplementationObjects) =>
      new TypeDefinition<T>(this, newImplementationObjects);

    public T Create(params object[] parameters) =>
      Compile().Create(parameters);

    public TypeDefinition<T> AddMethodMapper<TChild>(Func<T, TChild> creatorFunc) =>
      new TypeDefinition<T>(this, new object[] { new MethodMapperCreator<T, TChild>(creatorFunc) });
  }
}
