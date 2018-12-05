using System.Collections.Generic;
using System.Text;
using Sourceportal.Domain.Models.DB.Accounts;
using Sourceportal.Domain.Models.Shared;

namespace Sourceportal.Utilities
{
    public class AddressFormatter
    {
        private static string _lineBreak = "<br/>";
        private static string _commaSeparator = ", ";

        public static string FormatAddress(LocationDb dbLocation)
        {
            var formatted = new StringBuilder();

            var addressSections = new List<string>
            {
                dbLocation.Address1,
                dbLocation.Address2,
                FormatHouseNoStreet(dbLocation),
                dbLocation.Address4,
                GetCityStatePostal(dbLocation),
                dbLocation.District,
                dbLocation.CountryName
            };

            var seperator = dbLocation.LocationTypeId == (int) LocationTypesEnum.BillTo ? _lineBreak : _commaSeparator;
             
            foreach (var addressSection in addressSections)
            {
                if (!string.IsNullOrEmpty(addressSection))
                {
                    formatted.Append(addressSection);
                    formatted.Append(seperator);
                }
            }

            var formattedString = formatted.ToString();
            formattedString = formattedString.TrimEnd(seperator.ToCharArray());
            return formattedString;
        }

        private static string FormatHouseNoStreet(LocationDb dbLocation)
        {
            return string.Format("{0} {1}", dbLocation.HouseNumber, dbLocation.Street);
        }

        private static string GetCityStatePostal(LocationDb dbLocation)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(dbLocation.City))
            {
                sb.Append(dbLocation.City);
                AddCommaForFollowingContent(dbLocation, sb);
            }

            if (!string.IsNullOrEmpty(dbLocation.StateName))
            {
                sb.Append(" " + dbLocation.StateName);
            }

            if (!string.IsNullOrEmpty(dbLocation.PostalCode))
            {
                sb.Append(" " + dbLocation.PostalCode);
            }

            return sb.ToString();
        }

        private static void AddCommaForFollowingContent(LocationDb dbLocation, StringBuilder sb)
        {
            if (!string.IsNullOrEmpty(dbLocation.StateName) || !string.IsNullOrEmpty(dbLocation.PostalCode))
            {
                sb.Append(",");
            }
        }
    }
}