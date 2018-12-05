using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class BasicDetailsOptions : BaseResponse
    {
        [DataMember(Name = "statusList")]
        public List<AccountStatus> StatusList;

        [DataMember(Name = "accountTypes")]
        public List<AccountType> AccountTypes;

        [DataMember(Name = "companyTypes")]
        public List<CompanyType> CompanyTypes;
    }
}
