using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class SignatureChecker<T>
  {
    private readonly Dictionary<string, Type> properties;
    private readonly MethodInfo[] methods;

    public SignatureChecker()
    {
      Type type = typeof(T);
      properties = type.GetProperties().ToDictionary(info => info.Name, info => info.PropertyType);
      methods = type.GetMethods();
    }

    public void CheckSignatures(object[] wrappers)
    {
      foreach (object definition in wrappers)
      {
        switch (definition)
        {
          case Property property:
            CheckProperty(property.Name, property.PropertyType);
            break;
          case Method method:
            //CheckMethod(method.Name, method..PropertyType);
            break;
        }
      }
    }

    private void CheckProperty(string name, Type actualType)
    {
      if (properties.TryGetValue(name, out Type type) && type != actualType)
        throw new NotImplementedException($"Types mismatch on property {name}, expected: {type}, actual: {actualType}");
    }
  }
}