using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Accounts
{
    [DataContract]
   public class AccountsFilter
    {
        [DataMember(Name = "accountId")]
        public int? AccountId { get; set; }

        [DataMember(Name = "rowOffset")]
        public int? RowOffset { get; set; }

        [DataMember(Name = "rowLimit")]
        public int? RowLimit { get; set; }

        [DataMember(Name = "descSort")]
        public Boolean? DescSort { get; set; }

        [DataMember(Name = "sortBy")]
        public string SortBy { get; set; }

        [DataMember(Name = "searchString")]
        public string SearchString { get; set; }

        [DataMember(Name = "accountType")]
        public int? AccountType { get; set; }

        [DataMember(Name = "objectTypeId")]
        public int? ObjectTypeId { get; set; }
        
    }

    public class AccountFilter
    {
        public string SearchString { get; set; }
        public int AccountId { get; set; }
        public int RowOffset { get; set; }
        public int RowLimit { get; set; }
        public bool DescSort { get; set; }
        public string SortBy { get; set; }
        public string FilterBy { get; set; }
        public string FilterText { get; set; }
        public int AccountTypeId { get; set; }
        public bool AccountIsActive { get; set; }

    }
}
