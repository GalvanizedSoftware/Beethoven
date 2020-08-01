using System;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using static System.Reflection.Assembly;

namespace GalvanizedSoftware.Beethoven
{
  public class TypeDefinition<T> : TypeDefinition where T : class
  {
    private readonly PartDefinitions partDefinitions;
    private readonly string className;
    private readonly string classNamespace;
    private static readonly Assembly mainAssembly = typeof(T).Assembly;

    internal TypeDefinition(PartDefinitions partDefinitions, string classNamespace, string className)
    {
      this.partDefinitions = partDefinitions;
      this.className = className ?? $"{typeof(T).GetFormattedName()}Implementation";
      this.classNamespace = classNamespace ?? $"{typeof(T).Namespace}";
    }

    public TypeDefinition(params object[] newPartDefinitions) :
      this(new PartDefinitions(newPartDefinitions), null, null)
    {
    }

    public TypeDefinition(string name, params object[] newPartDefinitions) :
      this(new PartDefinitions(newPartDefinitions), null, name)
    {
    }

    private TypeDefinition(TypeDefinition<T> previousDefinition, object[] newPartDefinitions) :
      this(previousDefinition.partDefinitions.Concat(newPartDefinitions),
           previousDefinition.classNamespace, 
           previousDefinition.className)
    {
    }

    public TypeDefinition<T> Add(params object[] newImplementationObjects) =>
       new TypeDefinition<T>(this, newImplementationObjects);

    public TypeDefinition<T> AddMethodMapper<TChild>(Func<T, TChild> creatorFunc) =>
      new TypeDefinition<T>(this, new object[] { new MethodMapperCreator<T, TChild>(creatorFunc) });

    public CompiledTypeDefinition<T> Compile() =>
      CompileInternal(GetCallingAssembly());

    public T Create(params object[] parameters) =>
      CompileInternal(GetCallingAssembly()).Create(parameters);

    internal CompiledTypeDefinition<T> CompileInternal(Assembly callingAssembly)
    {
      BoundTypeDefinitionOfT<T> boundTypeDefinition = BindDefinition();
      AssemblyDefinition assemblyDefinition = new AssemblyDefinition().Add(boundTypeDefinition);
      Assembly assembly = assemblyDefinition.GenerateAssembly(mainAssembly, callingAssembly);
      return boundTypeDefinition.Link(assembly);
    }

    private BoundTypeDefinitionOfT<T> BindDefinition() =>
      new BoundTypeDefinitionOfT<T>(className, classNamespace, partDefinitions);

    internal T CreateInternal(Assembly callingAssembly, params object[] parameters) =>
      CompileInternal(callingAssembly).Create(parameters);
  }

  public abstract class TypeDefinition
  {
    internal protected TypeDefinition()
    {
    }
  }
}
