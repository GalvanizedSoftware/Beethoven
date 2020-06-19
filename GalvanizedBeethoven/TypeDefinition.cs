using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using static System.Reflection.Assembly;

namespace GalvanizedSoftware.Beethoven
{
  public class TypeDefinition<T> where T : class
  {
    private readonly PartDefinitions partDefinitions;

    internal TypeDefinition(PartDefinitions partDefinitions)
    {
      this.partDefinitions = partDefinitions;
    }

    public TypeDefinition(params object[] newPartDefinitions)
    {
      partDefinitions = new PartDefinitions(newPartDefinitions);
    }

    private TypeDefinition(TypeDefinition<T> previousDefinition, object[] newPartDefinitions) :
      this(previousDefinition.partDefinitions.Concat(newPartDefinitions))
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
      Type type = typeof(T);
      partDefinitions.SetMainTypeUser<T>();
      object[] allPartDefinitions = partDefinitions.GetAll<T>();
      IDefinition[] definitions = allPartDefinitions
        .GetAllDefinitions();
      string className = $"{type.GetFormattedName()}Implementation";
      string code = new ClassGenerator(type, className, definitions)
        .Generate()
        .Format(0);
      Type compiledType = new Compiler(code, new AssemblyCache<T>(callingAssembly))
        .GenerateAssembly().GetType($"{type.Namespace}.{className}");
      return new CompiledTypeDefinition<T>(compiledType, new BindingParents(allPartDefinitions));
    }

    internal T CreateInternal(Assembly callingAssembly, params object[] parameters) =>
      CompileInternal(callingAssembly).Create(parameters);
  }
}
