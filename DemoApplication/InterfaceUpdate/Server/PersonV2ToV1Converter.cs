using System;
using System.Globalization;

namespace GalvanizedSoftware.Beethoven.DemoApp.InterfaceUpdate.Server
{
  internal class PersonV2ToV1Converter
  {
    private readonly InterfaceUpdateV2.IPerson personV2;
    private readonly CultureInfo cultureInfo;

    public PersonV2ToV1Converter(InterfaceUpdateV2.IPerson personV2, CultureInfo cultureInfo)
    {
      this.personV2 = personV2;
      this.cultureInfo = cultureInfo;
    }

    internal string GetBirthDateString()
    {
      return personV2.BirthDate.ToString("d", cultureInfo);
    }

    internal void SetBirthDateDateTime(string dateTimeString)
    {
      personV2.BirthDate = DateTime.Parse(dateTimeString, cultureInfo);
    }
  }
}