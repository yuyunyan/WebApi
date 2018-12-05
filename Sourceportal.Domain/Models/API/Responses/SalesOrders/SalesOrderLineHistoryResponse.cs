using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.SalesOrders
{
    
        [DataContract]
        public class SalesOrderLineListResponse
        {
            [DataMember(Name = "soLineList")]
            public List<SalesOrderLineResponse> SOLineList { get; set; }
        }

        [DataContract]
        public class SalesOrderLineResponse
        {
            [DataMember(Name = "soLineId")]
            public int SOLineId { get; set; }

            [DataMember(Name = "lineNum")]
            public int LineNum { get; set; }

            [DataMember(Name = "salesOrderID")]
            public int SalesOrderID { get; set; }

            [DataMember(Name = "soExternalId")]
            public int SOExternalID { get; set; }

            [DataMember(Name = "versionId")]
            public int VersionId { get; set; }

            [DataMember(Name = "accountId")]
            public int AccountId { get; set; }

            [DataMember(Name = "accountName")]
            public string AccountName { get; set; }

            [DataMember(Name = "contactId")]
            public int ContactId { get; set; }

            [DataMember(Name = "firstName")]
            public string FirstName { get; set; }

            [DataMember(Name = "lastName")]
            public string LastName { get; set; }

            [DataMember(Name = "fullName")]
            public string FullName { get; set; }

            [DataMember(Name = "statusId")]
            public int StatusId { get; set; }

            [DataMember(Name = "statusName")]
            public string StatusName { get; set; }


            [DataMember(Name = "partNumber")]
            public string PartNumber { get; set; }

            [DataMember(Name = "manufacturer")]
            public string Manufacturer { get; set; }

            [DataMember(Name = "qty")]
            public int Qty { get; set; }

            [DataMember(Name = "price")]
            public decimal Price { get; set; }

            [DataMember(Name = "cost")]
            public decimal Cost { get; set; }

            [DataMember(Name = "gpm")]
            public decimal GPM { get; set; }

            [DataMember(Name = "datecode")]
            public string DateCode { get; set; }

            [DataMember(Name = "packaging")]
            public string Packaging { get; set; }

            [DataMember(Name = "orderDate")]
            public DateTime? OrderDate { get; set; }

            [DataMember(Name = "shipDate")]
            public DateTime? ShipDate { get; set; }

            [DataMember(Name = "owners")]
            public string Owners { get; set; }
        }

    }

