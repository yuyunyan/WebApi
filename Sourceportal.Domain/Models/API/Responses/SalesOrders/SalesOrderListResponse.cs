using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.SalesOrders
{

    [DataContract]
    public class SalesOrderListResponse : BaseResponse
    {
        [DataMember(Name = "salesOrders")]
        public IList<SalesOrderResponse> SalesOrders { get; set; }

        [DataMember(Name = "totalRowCount")]
        public int TotalRowCount;


    }

    [DataContract]
    public class SalesOrderResponse
    {

        [DataMember(Name = "salesOrderId")]
        public int SalesOrderID { get; set; }

        [DataMember(Name = "versionId")]
        public int VersionID { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountID { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "contactId")]
        public int ContactID { get; set; }

        [DataMember(Name = "contactFirstName")]
        public string ContactFirstName { get; set; }

        [DataMember(Name = "contactLastName")]
        public string ContactLastName { get; set; }

        [DataMember(Name = "statusId")]
        public int StatusID { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "countryName")]
        public string CountryName { get; set; }

        [DataMember(Name = "organizationId")]
        public int OrganizationID { get; set; }

        [DataMember(Name = "organizationName")]
        public string OrganizationName { get; set; }

        [DataMember(Name = "orderDate")]
        public string OrderDate { get; set; }

        [DataMember(Name = "owners")]
        public string Owners { get; set; }

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }

        [DataMember(Name = "incotermLocation")]
        public string IncotermLocation { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }
    }
}
