using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    [DataContract]
    public class ItemSalesOrdersResponse
    {
        [DataMember(Name = "items")]
        public List<ItemSalesOrdersDetails> ItemSalesOrders { get; set; }

        [DataMember(Name = "totalRowCount")]
        public int TotalRowCount;
    }

    [DataContract]
    public class ItemSalesOrdersDetails
    {

        [DataMember(Name = "soLineId")]
        public int SOLineID { get; set; }

        [DataMember(Name = "salesOrderId")]
        public int SalesOrderID { get; set; }

        [DataMember(Name = "versionId")]
        public int VersionID { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountID { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "orgName")]
        public string OrgName { get; set; }

        [DataMember(Name = "contactId")]
        public int ContactID { get; set; }

        [DataMember(Name = "orderDate")]
        public string OrderDate { get; set; }

        [DataMember(Name = "shipDate")]
        public string ShipDate { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "quantity")]
        public int Quantity { get; set; }

        [DataMember(Name = "price")]
        public string Price { get; set; }

        [DataMember(Name = "cost")]
        public string Cost { get; set; }
      
        [DataMember(Name = "packagingId")]
        public int PackagingID { get; set; }

        [DataMember(Name = "packagingName")]
        public string PackagingName { get; set; }

        [DataMember(Name = "packageConditionId")]
        public int PackageConditionID { get; set; }

        [DataMember(Name = "conditionName")]
        public string ConditionName { get; set; }

        [DataMember(Name = "owners")]
        public string Owners { get; set; }

        [DataMember(Name = "soExternalId")]
        public string SOExternalID { get; set; }
    }
}
