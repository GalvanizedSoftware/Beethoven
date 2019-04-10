using System;
using System.Globalization;
using GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdate.InterfacesV2;

namespace GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdate.Server
{
  internal class PersonV2ToV1Converter
  {
    private readonly IPerson personV2;
    private readonly CultureInfo cultureInfo;

    public PersonV2ToV1Converter(IPerson personV2, CultureInfo cultureInfo)
    {
      this.personV2 = personV2;
      this.cultureInfo = cultureInfo;
    }

    internal string GetBirthDateString() => 
      personV2.BirthDate.ToString("d", cultureInfo);

    internal void SetBirthDateDateTime(string dateTimeString) => 
      personV2.BirthDate = DateTime.Parse(dateTimeString, cultureInfo);
  }
}