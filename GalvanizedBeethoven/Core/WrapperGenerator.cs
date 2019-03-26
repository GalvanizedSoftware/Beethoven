using System;
using GalvanizedSoftware.Beethoven.Core.Methods;
using GalvanizedSoftware.Beethoven.Core.Properties;
using GalvanizedSoftware.Beethoven.Generic.Properties;
using System.Collections;
using System.Collections.Generic;
using GalvanizedSoftware.Beethoven.Generic;
using GalvanizedSoftware.Beethoven.Generic.Methods;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class WrapperGenerator<T> : IEnumerable<object> where T : class
  {
    private readonly object[] partDefinitions;
    private readonly Func<object, IEnumerable<object>> getWrappers;

    public WrapperGenerator(object[] partDefinitions,
      Func<object, IEnumerable<object>> getWrappers)
    {
      this.partDefinitions = partDefinitions;
      this.getWrappers = getWrappers;
    }

    public IEnumerator<object> GetEnumerator()
    {
      foreach (object definition in partDefinitions)
      {
        switch (definition)
        {
          case null:
            break;
          case Property property:
            yield return property;
            break;
          case Method method:
            yield return method;
            break;
          case IEnumerable<Property> properties:
            foreach (Property property in properties)
              yield return property;
            break;
          case IEnumerable<Method> methods:
            foreach (Method method in methods)
              yield return method;
            break;
          case DefaultProperty _:
          case DefaultMethod _:
            // Dependent on what other wrappers are in there, so it has to be evaluated last
            break;
          case LinkedObjects linkedObjects:
            foreach (object wrapper in linkedObjects.GetWrappers<T>())
              yield return wrapper;
            break;
          default:
            foreach (object wrapper in getWrappers(definition))
              yield return wrapper;
            break;
        }
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}