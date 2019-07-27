using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Extensions;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal class InterceptorsMap
  {
    private readonly Dictionary<MethodInfo, IGeneralInterceptor> interceptorsMap =
      new Dictionary<MethodInfo, IGeneralInterceptor>();

    public IEnumerable<object> Values => interceptorsMap.Values;

    public void Add(MethodInfo methodInfo, IGeneralInterceptor interceptor)
    {
      if (interceptorsMap.TryGetValue(methodInfo, out IGeneralInterceptor currentInterceptor))
        interceptorsMap[methodInfo] = new CompositeInterceptor(currentInterceptor, interceptor);
      else
        interceptorsMap.Add(methodInfo, interceptor);
    }

    public IGeneralInterceptor Find(MethodInfo methodInfo) =>
      interceptorsMap
        .Where(pair => CompareMethodInfos(pair.Key, methodInfo))
        .Select(pair => pair.Value)
        .SingleOrDefault();

    private static bool CompareMethodInfos(MethodInfo methodInfo1, MethodInfo methodInfo2) =>
      !methodInfo1.IsGenericMethod ? 
        methodInfo1 == methodInfo2:
        methodInfo1
          .GetParameterTypesIgnoreGeneric()
          .SequenceEqual(methodInfo2.GetParameterTypesIgnoreGeneric()) &&
        methodInfo1.ReturnType.IsMatchReturnTypeIgnoreGeneric(methodInfo2);
  }
}