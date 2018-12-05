using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Images
{
    [DataContract]
    public class FileResponse
    {
        [DataMember(Name = "files")]
        public IList<File> Files { get; set; }
    }

    [DataContract]
    public class File
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}


