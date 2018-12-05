using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Images
{
    public class DocumentDbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            //DocumentSet
            {-1, "ObjectTypeID Missing"},
            {-2, "ObjectID Missing"},
            {-3, "FileNameStored Missing"},
            {-4, "Invalid DocumentID"},
            //DocumentGet
            {-5, "Missing ObjectTypeId or ObjectID"},
            //DocumentDelete
            {-6, "Document delete failed. Document ID is missing."}
        };
    }
}
