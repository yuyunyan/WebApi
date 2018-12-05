using System;
using System.Runtime.Serialization;
using Sourceportal.Domain.Models.Middleware.Enums;
using Sourceportal.Domain.Models.Middleware.SalesOrder;

namespace Sourceportal.Domain.Models.Middleware
{
    public class MiddlewareSyncRequest<T> where T: MiddlewareSyncBase
    {
        private string _direction = MiddlewareSyncDirections.Outgoing.ToString();
        private string _action = MiddlewareActionType.Sync.ToString();

        public MiddlewareSyncRequest(int objectId, string objectType, int createdBy, int objectTypeId)
        {
            ObjectId = objectId;
            ObjectType = objectType;
            CreatedBy = createdBy;
            ObjectTypeId = objectTypeId;
        }

        [DataMember(Name = "objectId")]
        public int ObjectId { get; private set; }

        [DataMember(Name = "objectType")]
        public string ObjectType { get; private set; }

        [DataMember(Name = "action")]
        public string Action {get { return _action; } set { _action = value; } }

        [DataMember(Name = "data")]
        public T Data { get; set; }

        [DataMember(Name = "direction")]
        public string Direction { get { return _direction; } set { _direction = value; } }

        [DataMember(Name = "createdBy")]
        public int CreatedBy { get; private set; }

        [DataMember(Name = "source")]
        public string Source { get { return "RFQ"; } }

        [DataMember(Name = "creator")]
        public string Creator { get; set; } 

        [DataMember(Name = "owners")]
        public string Owners { get; set; }

        [IgnoreDataMember]
        public int ObjectTypeId { get; private set; }
    }

    public class MiddlewareSyncRequest: MiddlewareSyncBase
    {
       
        public MiddlewareSyncRequest(int objectId, string objectType, int createdBy, int objectTypeId, string externalId) :base(objectId, externalId)
        {
            ObjectId = objectId;
            ObjectType = objectType;
            CreatedBy = new CreatedBy()
            {
                Id = createdBy
            };
            ObjectTypeId = objectTypeId;
        }

        [DataMember(Name = "objectId")]
        public int ObjectId { get; private set; }

        [DataMember(Name = "objectType")]
        public string ObjectType { get; private set; }       

        [DataMember(Name = "createdBy")]
        public CreatedBy CreatedBy { get; set; }

        [DataMember(Name = "source")]
        public string Source { get { return "RFQ"; } }        

        [DataMember(Name = "owners")]
        public string Owners { get; set; }

        [IgnoreDataMember]
        public int ObjectTypeId { get; private set; }
    }
}
