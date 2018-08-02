namespace GalvanizedSoftware.Beethoven.DemoApp.Mapped
{
  public class UsAddress : IAddress
  {
    public string Adddress1 { get; set; }
    public string Adddress2 { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Zip { get; set; }
    public string County { get; set; }
    public string Country { get; } = "USA";

    public string FullAddress => AddressFormatter.FormatAddress(
      Adddress1,
      Adddress2,
      $"{City}, {State} {Zip}",
      County,
      Country);
  }
}
