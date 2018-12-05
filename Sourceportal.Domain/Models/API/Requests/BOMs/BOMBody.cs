using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.BOMs
{
    [DataContract]
    public class BOMBody
    {
        [DataMember(Name = "accountId")]
        public int AccountID { get; set; }

        [DataMember(Name = "contactId")]
        public int ContactID { get; set; }

        [DataMember(Name = "salesUserId")]
        public int SalesUserID { get; set; }

        [DataMember(Name = "currencyId")]
        public string CurrencyId { get; set; }

        [DataMember(Name = "sourcingTypeId")]
        public int SourcingTypeID { get; set; }

        [DataMember(Name = "publishToSources")]
        public bool PublishToSources { get; set; }
         
        [DataMember(Name = "runMatch")]
        public bool RunMatch { get; set; }

        [DataMember(Name = "listName")]
        public string ListName { get; set; }

        [DataMember(Name = "listTypeId")]
        public int ListTypeID { get; set; }

        [DataMember(Name = "saveLayout")]
        public bool SaveLayout { get; set; }

        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        [DataMember(Name = "xlsType")]
        public string XlsType { get; set; }
        [DataMember(Name = "quoteTypeId")]
        public int? QuoteTypeId { get; set; }

    }
}
