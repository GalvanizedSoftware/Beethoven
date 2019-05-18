using System;
using GalvanizedSoftware.Beethoven.Core.Methods;

namespace GalvanizedSoftware.Beethoven.Generic
{
  /// <summary>
  /// Helper class to avoid too many generic-type specification
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class FactoryHelper<T> where T : class
  {
    public MethodMapperCreator<T, TChild> MethodMapper<TChild>(Func<T, TChild> creatorFunc)
    {
      return new MethodMapperCreator<T, TChild>(creatorFunc);
    }
  }
}