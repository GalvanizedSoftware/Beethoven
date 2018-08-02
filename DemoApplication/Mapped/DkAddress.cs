namespace GalvanizedSoftware.Beethoven.DemoApp.Mapped
{
  public class DkAddress : IAddress
  {
    public string Adddress1 { get; set; }
    public string Adddress2 { get; set; }
    public string City { get; set; }
    public string Zip { get; set; }
    public string Country { get; } = "Denmark";

    public string FullAddress => AddressFormatter.FormatAddress(
        Adddress1,
        Adddress2,
        $"{Zip} {City}",
        Country);
  }
}
