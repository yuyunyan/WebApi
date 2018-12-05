using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.BOMs
{
    public class BOMListDbs
    {
        public int ItemListID { get; set; }
        
        public string ListName { get; set; }
        
        public int AccountID { get; set; }
        
        public string AccountName { get; set; }
        
        public int ContactID { get; set; }
        
        public string ContactName { get; set; }
        
        public int ItemCount { get; set; }
        
        public string FileNameOriginal { get; set; }
        
        public int StatusID { get; set; }
        
        public string StatusName { get; set; }
        
        public string UserName { get; set; }
        
        public string Created { get; set; }
        
        public string OrganizationName { get; set; }

        public int Comments { get; set; }

        public int RowCount { get; set; }
    }
}
