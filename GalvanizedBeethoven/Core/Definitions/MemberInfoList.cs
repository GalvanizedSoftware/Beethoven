using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Definitions
{
  internal class MemberInfoList
  {
    internal MemberInfoList(Type interfaceType)
    {
      MemberInfo[] membersInfos = interfaceType
        .GetAllTypes()
        .SelectMany(type => type.GetMembers())
        .ToArray();
      PropertyInfos = membersInfos
        .OfType<PropertyInfo>()
        .ToArray();
      MethodInfos = membersInfos
        .OfType<MethodInfo>()
        .Where(methodInfo => !methodInfo.IsSpecialName)
        .ToArray();
      MethodIndexes = MethodInfos
        .ToDictionary(
          methodInfo => methodInfo,
          FindIndex);
      EventInfos = membersInfos
        .OfType<EventInfo>()
        .ToArray();
    }

    public PropertyInfo[] PropertyInfos { get; }

    public MethodInfo[] MethodInfos { get; }

    public Dictionary<MethodInfo, int> MethodIndexes { get; }

    public EventInfo[] EventInfos { get; }

    private int FindIndex(MethodInfo methodInfo)
    {
      string name = methodInfo.Name;
      MethodInfo[] methodInfoArray = MethodInfos
        .Where(info => info.Name == name)
        .ToArray();
      return methodInfoArray.Length == 1 ? 0 :
        methodInfoArray
          .Select((info, index) => (info, index))
          .Where(tuple => tuple.info == methodInfo)
          .Select(tuple => tuple.index)
          .FirstOrDefault();
    }

    public string GetMethodInvokerName(MethodInfo methodInfo) => 
      $"invoker{methodInfo?.Name}{MethodIndexes.TryGetValue(methodInfo)}";
  }
}