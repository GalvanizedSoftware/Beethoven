using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Extensions;
using GalvanizedSoftware.Beethoven.Generic;

namespace GalvanizedSoftware.Beethoven.Core.Interceptors
{
  internal abstract class PropertyInterceptor : IGeneralInterceptor
  {
    private Parameter parameter;

    internal Property Property { get; }

    protected PropertyInterceptor(Property property)
    {
      MainDefinition = Property = property;
      //object definition = this;
      //parameter = typeof(Parameter<>).Create1(Property.PropertyType, definition);
    }

    public void Invoke(InstanceMap instanceMap, Action<object> returnAction, object[] parameters, Type[] genericArguments,
      MethodInfo methodInfo)
    {
      if (!Property.IsMatch(methodInfo))
        throw new NotImplementedException();
      InvokeIntercept(null, returnAction, parameters);
    }

    public object MainDefinition { get; }

    protected abstract void InvokeIntercept(object localInstance, Action<object> returnAction, object[] parameters);
  }
}