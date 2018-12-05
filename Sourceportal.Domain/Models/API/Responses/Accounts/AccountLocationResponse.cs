﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class AccountLocationResponse
    {
        [DataMember(Name = "locations")]
        public IList<Location> Locations;
    }

    [DataContract]
    public class Location
    {
        [DataMember(Name = "locationId")]
        public int LocationId;

        [DataMember(Name = "accountId")]
        public int AccountId;

        [DataMember(Name = "externalId")]
        public string ExternalId;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "locationTypeExternalIds")]
        public List<string> LocationTypeExternalId;

        [DataMember(Name = "formattedAddress")]
        public string FormattedAddress;

        [DataMember(Name = "typeId")]
        public int TypeId;

        [DataMember(Name = "locationTypeName")]
        public string LocationTypeName;

        [DataMember(Name = "countryId")]
        public int CountryId;

        [DataMember(Name = "addressLine1")]
        public string AddressLine1;

        [DataMember(Name = "countryCode")]
        public string CountryCode;

        [DataMember(Name = "countryCode2")]
        public string CountryCode2;

        [DataMember(Name = "addressLine2")]
        public string AddressLine2;

        [DataMember(Name = "houseNo")]
        public string HouseNo;

        [DataMember(Name = "street")]
        public string Street;

        [DataMember(Name = "addressLine4")]
        public string AddressLine4;

        [DataMember(Name = "city")]
        public string City;

        [DataMember(Name = "stateId")]
        public int StateId;

        [DataMember(Name = "stateCode")]
        public string StateCode;

        [DataMember(Name = "stateName")]
        public string StateName;

        [DataMember(Name = "formattedState")]
        public string FormattedState;

        [DataMember(Name = "postalCode")]
        public string PostalCode;

        [DataMember(Name = "district")]
        public string District;

        [DataMember(Name = "Note")]
        public string Note;

        [DataMember(Name = "isDeleted")]
        public bool IsDeleted;

        [DataMember(Name = "shipToChecked")]
        public bool shipToChecked;

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage;
    }
}
