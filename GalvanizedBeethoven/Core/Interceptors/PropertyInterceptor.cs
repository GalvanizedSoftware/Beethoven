using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal abstract class PropertyInterceptor : IGeneralInterceptor
  {
    internal PropertyDefinition PropertyDefinition { get; }

    protected PropertyInterceptor(PropertyDefinition propertyDefinition)
    {
      MainDefinition = PropertyDefinition = propertyDefinition;
    }

    public void Invoke(InstanceMap instanceMap, ref object returnValue, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo)
    {
      if (!PropertyDefinition.IsMatch(methodInfo))
        throw new NotImplementedException();
      InvokeIntercept(instanceMap, ref returnValue, parameters);
    }

    public object MainDefinition { get; }

    protected abstract void InvokeIntercept(InstanceMap instanceMap, ref object returnValue, object[] parameters);
  }
}