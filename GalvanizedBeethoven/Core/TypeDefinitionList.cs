using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core
{
  public static class TypeDefinitionList
  {
    private static readonly Dictionary<string, object> list = new();

    public static TypeDefinition<T> Get<T>(string id) where T : class
    {
      list.TryGetValue(id, out object value);
      return value as TypeDefinition<T>;
    }

    public static void Add<T>(string id, TypeDefinition<T> instance) where T : class =>
      list.Add(id, instance);
  }
}
