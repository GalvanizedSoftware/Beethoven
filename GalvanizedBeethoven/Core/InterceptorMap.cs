using System;
using System.Reflection;
using GalvanizedSoftware.Beethoven.Core.Interceptors;

namespace GalvanizedSoftware.Beethoven.Core
{
  public class InterceptorMap : Tuple<MethodInfo, IGeneralInterceptor>
  {
    public InterceptorMap(MethodInfo item1, IGeneralInterceptor item2) : 
      base(item1, item2)
    {
    }
  }
}