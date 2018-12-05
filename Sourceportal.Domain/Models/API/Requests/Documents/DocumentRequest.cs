using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Documents
{
    public class DocumentListRequest
    {
        public IList<DocumentRequest> documents { get; set; }
    }
    
    public class DocumentRequest
    {
     public int DoucmentID { get; set; }
     public int ObjectTypeID { get; set; }
     public int ObjectID { get; set; }
     public string DocName { get; set; }
     public string FileNameOriginal { get; set; }
     public string FileNameStored { get; set; }
     public string FileMimeType { get; set; }
     public string FolderPath { get; set; }
     public bool IsDeleted { get; set; }
    }
}
