using System;
using System.Reflection;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  public interface IGeneralInterceptor
  {
    void Invoke(InstanceMap instanceMap, ref object returnValue, object[] parameters, Type[] genericArguments, MethodInfo methodInfo);
    object MainDefinition { get; }
  }
}
