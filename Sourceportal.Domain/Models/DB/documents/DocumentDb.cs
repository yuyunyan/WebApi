using System;

namespace Sourceportal.Domain.Models.DB.documents
{
    public class DocumentDb
    {
        public int DocumentId;
        public int ObjectTypeID;
        public int ObjectID;
        public string DocName;
        public string FolderPath;
        public string FileNameOriginal;
        public string FileNameStored;
        public string FileMimeType;
        public string Created;
        public int TotalRows;
        public bool IsSytem;
        public string ExternalId;
    }
}
