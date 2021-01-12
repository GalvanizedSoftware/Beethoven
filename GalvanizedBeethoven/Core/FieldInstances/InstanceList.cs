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
      new(new(factoryDefinition.PartDefinitions));

    public static InstanceList<T> Create(string id) =>
      new(TypeDefinitionList.Get<T>(id).PartDefinitions);

    private readonly Dictionary<string, object> instances = new();

    private InstanceList(PartDefinitions partDefinitions)
    {
      new LinkedDefinitions<T>(partDefinitions)
        .FieldMaps
        .SelectMany(fieldMaps => fieldMaps.GetFields())
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