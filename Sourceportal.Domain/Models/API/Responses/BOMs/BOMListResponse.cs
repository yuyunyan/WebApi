using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.BOMs
{
    [DataContract]
    public class BOMListResponse
    {
        [DataMember(Name = "bomList")]
        public List<BOMList> BomList { get; set; }

        [DataMember(Name = "rowCount")]
        public int RowCount { get; set; }
    }

    [DataContract]
    public class BOMList
    {
        [DataMember(Name = "itemListId")]
        public int ItemListId { get; set; }

        [DataMember(Name = "listName")]
        public string ListName { get; set; }

        [DataMember(Name = "accountID")]
        public int AccountID { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "contactID")]
        public int ContactID { get; set; }

        [DataMember(Name = "contactName")]
        public string ContactName { get; set; }

        [DataMember(Name = "itemCount")]
        public int ItemCount { get; set; }

        [DataMember(Name = "fileName")]
        public string FileName { get; set; }

        [DataMember(Name = "statusID")]
        public int StatusID { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "userName")]
        public string UserName { get; set; }

        [DataMember(Name = "loadDate")]
        public string LoadDate { get; set; }

        [DataMember(Name = "organizationName")]
        public string OrganizationName { get; set; }

        [DataMember(Name = "comments")]
        public int Comments { get; set; }
    }
}
