using System;

namespace GalvanizedSoftware.Beethoven.Generic.Properties
{
  public static class LazyCreatorFactory
  {
    public static LazyCreator<T> CreateIfMatch<T>(Type type, Func<object> creatorFunc) => 
      type != typeof(T) ? null : new LazyCreator<T>(() => (T)creatorFunc());
  }
}