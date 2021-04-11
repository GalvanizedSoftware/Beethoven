namespace GalvanizedSoftware.Beethoven.Test.VariousTests.EqualsGetHashImport
{
  public interface IValueHolder : IEqualsGetHash // We can only pass one interface to BeethovenFactory (for now), so we need to combine the interfaces
  {
    string Name { get; set; }
    int Value { get; set; }
    byte[] Data { get; set; }
  }
}
