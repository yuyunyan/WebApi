using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Documents
{
    public class DocumentListResponse : BaseResponse
    {
        [DataMember(Name = "documents")]
        public IList<DocumentResponse> Documents { get; set; }

        [DataMember(Name = "totalRowCount")]
        public int RowCount;
    }

    public class DocumentResponse
    {
        [DataMember(Name = "documentId")]
        public int DocumentID { get; set; }

        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }

        [DataMember(Name = "objectId")]
        public int ObjectID { get; set; }

        [DataMember(Name = "docName")]
        public string DocName { get; set; }

        [DataMember(Name = "FolderPath")]
        public string FolderPath { get; set; }

        [DataMember(Name = "fileNameOriginal")]
        public string FileNameOriginal { get; set; }

        [DataMember(Name = "fileNameStored")]
        public string FileNameStored { get; set; }

        [DataMember(Name = "fileMimeType")]
        public string FileMimeType { get; set; }

        [DataMember(Name = "created")]
        public string Created { get; set; }
    }

}
