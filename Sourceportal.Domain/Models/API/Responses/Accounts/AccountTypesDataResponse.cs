using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class AccountTypesDataResponse
    {
        [DataMember(Name = "types")]
        public List<AccountTypesData> Types { get; set; }
    }

    [DataContract]
    public class AccountTypesData
    {
        [DataMember(Name ="accountTypeId")]
        public int AccountTypeID { get; set; }

        [DataMember(Name = "accountStatusId")]
        public int AccountStatusID { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "paymentTermId")]
        public int PaymentTermID { get; set; }

        [DataMember(Name = "paymentTermName")]
        public string PaymentTermName { get; set; }

        [DataMember(Name = "epdsId")]
        public string EPDSID { get; set; }
    }
}
