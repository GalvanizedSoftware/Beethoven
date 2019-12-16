using GalvanizedSoftware.Beethoven.Generic.Parameters;

namespace GalvanizedSoftware.Beethoven.Generic
{
  public interface IInstanceMap
  {
    object GetLocal(IParameter parameter);
  }
}