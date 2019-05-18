using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal abstract class PropertyInterceptor : IGeneralInterceptor
  {
    internal Property Property { get; }

    protected PropertyInterceptor(Property property)
    {
      MainDefinition = Property = property;
    }

    public void Invoke(InstanceMap instanceMap, Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo)
    {
      if (!Property.IsMatch(methodInfo))
        throw new NotImplementedException();
      InvokeIntercept(instanceMap.GetLocal(this), returnAction, parameters);
    }

    public object MainDefinition { get; }

    protected abstract void InvokeIntercept(object localInstance, Action<object> returnAction, object[] parameters);
  }
}