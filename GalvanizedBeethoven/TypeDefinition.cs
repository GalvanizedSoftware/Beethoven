using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core;
using GalvanizedSoftware.Beethoven.Core.CodeGenerators;
using GalvanizedSoftware.Beethoven.Core.Definitions;
using GalvanizedSoftware.Beethoven.Core.FieldInstances;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;
using static System.Guid;
using static System.Reflection.Assembly;

namespace GalvanizedSoftware.Beethoven
{
  public class TypeDefinition<T> where T : class
  {
    public static TypeDefinition<T> Create(params object[] newPartDefinitions) =>
      new(new(newPartDefinitions), null, null, null);

    private readonly NameDefinition nameDefinition;
    private static readonly Assembly mainAssembly = typeof(T).Assembly;

    internal static TypeDefinition<T> CreateFromFactoryDefinition(IFactoryDefinition<T> factoryDefinition, MemberInfo factoryMemberInfo) =>
      factoryDefinition == null ? null :
        new TypeDefinition<T>(new (factoryDefinition.PartDefinitions),
          factoryDefinition.Namespace,
          factoryDefinition.ClassName,
          factoryMemberInfo);

    private TypeDefinition(PartDefinitions partDefinitions, string classNamespace, string className, MemberInfo factoryMemberInfo) :
      this(partDefinitions, GetNameDefinition(classNamespace, className), factoryMemberInfo)
    {
    }

    private static MemberInfoList MemberInfoList { get; } = MemberInfoListCache.Get<T>();

    internal PartDefinitions PartDefinitions { get; }

    private TypeDefinition(PartDefinitions partDefinitions, NameDefinition nameDefinition,
      MemberInfo factoryMemberInfo)
    {
      PartDefinitions = partDefinitions
        .Set(InstanceListFieldDefinition<T>.Create(factoryMemberInfo, Id));
      this.nameDefinition = nameDefinition;
      //PartDefinitions.SetMainTypeUser(typeof(T));
      TypeDefinitionList.Add(Id, this);
    }

    public string Id { get; } = NewGuid().ToString("N");

    private TypeDefinition(TypeDefinition<T> previousDefinition, object[] newPartDefinitions) :
      this(previousDefinition.PartDefinitions.Concat(newPartDefinitions),
           previousDefinition.nameDefinition, null)
    {
    }

    private static NameDefinition GetNameDefinition(string classNamespace, string className) =>
      new(
        className ?? $"{typeof(T).GetFormattedName()}Implementation",
        classNamespace ?? $"{typeof(T).Namespace}"
      );

    public TypeDefinition<T> Add(params object[] newImplementationObjects) =>
       new(this, newImplementationObjects);

    public TypeDefinition<T> AddMethodMapper<TChild>(Func<T, TChild> creatorFunc) =>
      new(this, new object[] { new MethodMapperCreator<T, TChild>(creatorFunc) });

    public CompiledTypeDefinition<T> Compile() =>
      CompileInternal(GetCallingAssembly());

    public T CreateNew(params object[] parameters) =>
      CompileInternal(GetCallingAssembly()).Create(parameters);

    internal TypeDefinitionGeneratorOfT<T> CreateGenerator() =>
      new(MemberInfoList, nameDefinition, PartDefinitions);

    internal CompiledTypeDefinition<T> CompileInternal(Assembly callingAssembly)
    {
      TypeDefinitionGeneratorOfT<T> generator = CreateGenerator();
      Assembly assembly = new AssemblyDefinition()
        .Add(generator)
        .GenerateAssembly(mainAssembly, callingAssembly);
      return new BoundTypeDefinitionOfT<T>(nameDefinition, generator).Link(assembly);
    }
  }
}
