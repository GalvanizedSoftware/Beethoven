using System;
using GalvanizedSoftware.Beethoven.Core.Interceptors;

namespace GalvanizedSoftware.Beethoven.Core
{
  internal class InterceptorMap : Tuple<string, IGeneralInterceptor>
  {
    public InterceptorMap(string item1, IGeneralInterceptor item2) : base(item1, item2)
    {
    }
  }
}