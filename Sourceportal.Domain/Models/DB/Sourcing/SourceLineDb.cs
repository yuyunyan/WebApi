using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Sourcing
{
   public class SourceLineDb
    {
        public string TypeName { get; set; }
        public string PartNumber { get; set; }
        public int ItemID { get; set; }
        public string Manufacturer { get; set; }
        public DateTime Created { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int Qty { get; set; }
        public decimal Cost { get; set; }
        public string DateCode { get; set; }
        public string PackagingName { get; set; }
        public int LeadTimeDays { get; set; }
        public string Owners { get; set; }
        public string ContactName { get; set; }
        public string CreatedBy { get; set; }
    }
}
