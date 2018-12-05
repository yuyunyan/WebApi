using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.PurchaseOrders
{
   public class SetPurchaseOrderDetailsRequest
    {
       public  int PurchaseOrderId { get; set; }
       public int VersionId { get; set; }
       public int AccountId { get; set; }
       public int ContactId { get; set; }
       public int StatusId { get; set; }
       public int FromLocationId { get; set; }
       public int ToWarehouseID { get; set; }
       public string CurrencyId { get; set; }
       public int ShippingMethodId { get; set; }
       public int PaymentTermId { get; set; }
       public  string OrderDate { get; set; }
       public  bool  IsDeleted { get; set; }
       public int IncotermId { get; set; }
       public int OrganizationId { get; set; }
        public string ExternalId { get; set; }
        public string PONotes { get; set; }
    }
}
