using System.Collections.Generic;
using System.Linq;
using GalvanizedSoftware.Beethoven.Generic;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class InstanceMap : IInstanceMap
  {
    private readonly Dictionary<Parameter, object> dictionary;

    public InstanceMap(IEnumerable<object> partDefinitions, object[] parameters)
    {
      dictionary = partDefinitions.OfType<Parameter>()
        .Zip(parameters, (parameter, instance) => (parameter, instance))
        .ToDictionary(
        tuple => tuple.parameter,
        tuple => tuple.instance);
    }

    public object GetLocal(Parameter parameter)
    {
      if (parameter == null)
        return null;
      KeyValuePair<Parameter, object> pair = dictionary
        .FirstOrDefault(item => item.Key.CompareTo(parameter) == 0);
      Parameter instanceParameter = pair.Key;
      if (instanceParameter == null)
        return null;
      object instance = pair.Value;
      if (instance != null)
        return instance;
      instance = instanceParameter.Create();
      dictionary[instanceParameter] = instance;
      return instance;
    }
  }
}