using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using static System.Reflection.Assembly;

namespace GalvanizedSoftware.Beethoven
{
  public class TypeDefinition<T> where T : class
  {
    public static TypeDefinition<T> Create(params object[] newPartDefinitions) =>
      new TypeDefinition<T>(new PartDefinitions(newPartDefinitions), null, null);

    public static TypeDefinition<T> CreateNamed(string className, params object[] newPartDefinitions) =>
      new TypeDefinition<T>(new PartDefinitions(newPartDefinitions), null, className);

    private readonly NameDefinition nameDefinition;
    private readonly PartDefinitions partDefinitions;
    private static readonly Assembly mainAssembly = typeof(T).Assembly;

    internal static TypeDefinition<T> CreateFromFactoryDefinition(IFactoryDefinition<T> factoryDefinition) =>
      factoryDefinition == null ? null :
        new TypeDefinition<T>(new PartDefinitions(
          factoryDefinition.PartDefinitions),
          factoryDefinition.Namespace,
          factoryDefinition.ClassName);

    private TypeDefinition(PartDefinitions partDefinitions, string classNamespace, string className) :
      this(partDefinitions, GetNameDefinition(classNamespace, className))
    {
    }

    private static NameDefinition GetNameDefinition(string classNamespace, string className) =>
      new NameDefinition(
        className ?? $"{typeof(T).GetFormattedName()}Implementation",
        classNamespace ?? $"{typeof(T).Namespace}"
      );

    private TypeDefinition(PartDefinitions partDefinitions, NameDefinition nameDefinition)
    {
      this.partDefinitions = partDefinitions;
      this.nameDefinition = nameDefinition;
      partDefinitions.SetMainTypeUser(typeof(T));
    }

    private TypeDefinition(TypeDefinition<T> previousDefinition, object[] newPartDefinitions) :
      this(previousDefinition.partDefinitions.Concat(newPartDefinitions),
           previousDefinition.nameDefinition)
    {
    }

    public TypeDefinition<T> Add(params object[] newImplementationObjects) =>
       new TypeDefinition<T>(this, newImplementationObjects);

    public TypeDefinition<T> AddMethodMapper<TChild>(Func<T, TChild> creatorFunc) =>
      new TypeDefinition<T>(this, new object[] { new MethodMapperCreator<T, TChild>(creatorFunc) });

    public CompiledTypeDefinition<T> Compile() =>
      CompileInternal(GetCallingAssembly());

    public T CreateNew(params object[] parameters) =>
      CompileInternal(GetCallingAssembly()).Create(parameters);

    internal CompiledTypeDefinition<T> CompileInternal(Assembly callingAssembly)
    {
      TypeDefinitionGeneratorOfT<T> generator = new TypeDefinitionGeneratorOfT<T>(nameDefinition, partDefinitions);
      Assembly assembly = new AssemblyDefinition()
        .Add(generator)
        .GenerateAssembly(mainAssembly, callingAssembly);
      return new BoundTypeDefinitionOfT<T>(nameDefinition, generator).Link(assembly);
    }
  }
}
