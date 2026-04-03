using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MagFlow.BLL.Helpers.Localization
{
    public static class CountryHelper
    {
        public static List<CountryItem> GetCountries()
        {
            return  CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Select(culture => new RegionInfo(culture.Name))
                .GroupBy(region => region.TwoLetterISORegionName)
                .Select(group => group.First())
                .OrderBy(region => region.DisplayName)
                .Select(region => new CountryItem
                {
                    Code = region.TwoLetterISORegionName,
                    Name = region.DisplayName
                })
                .ToList();
        }
    }
}
