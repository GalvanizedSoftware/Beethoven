using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Core.Definitions;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Interfaces;

namespace GalvanizedSoftware.Beethoven.Core.FieldInstances
{
  public class InstanceList<T> where T : class
  {
    public static InstanceList<T> Create(IFactoryDefinition<T> factoryDefinition) =>
      new(new PartDefinitions(factoryDefinition.PartDefinitions).GetAll<T>());

    internal static InstanceList<T> Create(PartDefinitions partDefinitions) =>
      new(new PartDefinitions(partDefinitions));

    public static InstanceList<T> Create(string id)
    {
      TypeDefinition<T> typeDefinition = TypeDefinitionList.Get<T>(id);
      return new(new PartDefinitions(typeDefinition.PartDefinitions));
    }

    private readonly Dictionary<string, object> instances = new();

    private InstanceList(PartDefinitions partDefinitions)
    {
      IDefinition[] allDefinitions = partDefinitions
        .GetAllDefinitions<T>()
        .ToArray();
      allDefinitions
        .OfType<IMainTypeUser>()
        .ForEach(user => user.Set(typeof(T)));
      allDefinitions
        .SelectMany(definition => definition.GetFields())
        .ForEach(SetInstance);
    }

    // ReSharper disable once UnusedMember.Global
    public TInstance GetInstance<TInstance>(string uniqueName) =>
      (TInstance)instances[uniqueName];

    internal void SetInstance(string uniqueName, object instance)
    {
      if (!instances.ContainsKey(uniqueName))
        instances.Add(uniqueName, instance);
    }
  }
}