using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Responses.CommonData;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
   [DataContract]
   public class QuoteOptionsResponse
   {
       [DataMember(Name = "customers")]
       public List<CustomerAccount> Customers;

       [DataMember(Name = "contacts")]
       public List<AccountContact> Contacts;

       [DataMember(Name = "shipAddress")]
       public List<AccountShipAddress> ShipAddress;

       [DataMember(Name = "status")]
       public List<Status> Status;
   }
}
