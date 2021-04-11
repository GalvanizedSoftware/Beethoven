using System;
using System.Collections.Generic;

namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
  internal static class MemberInfoListCache
  {
    private static readonly Dictionary<Type, MemberInfoList> cache = new();

    public static MemberInfoList Get<T>() where T : class => 
      Get(typeof(T));

    public static MemberInfoList Get(Type interfaceType) => 
      cache.TryGetValue(interfaceType, out MemberInfoList value) ? 
        value : 
        CreateAndAdd(interfaceType);

    private static MemberInfoList CreateAndAdd(Type interfaceType)
    {
      MemberInfoList value = new(interfaceType);
      cache.Add(interfaceType, value);
      return value;
    }
  }
}
