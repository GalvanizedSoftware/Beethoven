namespace GalvanizedSoftware.Beethoven.Core.Binding
{
  internal interface ITypeBinding<in TRequired>
  {
    void Bind(TRequired master);
  }
}