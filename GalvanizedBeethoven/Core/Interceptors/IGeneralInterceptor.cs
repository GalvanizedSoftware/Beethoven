using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal interface IGeneralInterceptor
  {
    void Invoke(InstanceMap instanceMap, Action<object> returnAction, object[] parameters, Type[] genericArguments, MethodInfo methodInfo);
    object MainDefinition { get; }
  }
}
