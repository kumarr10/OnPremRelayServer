using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class UserRequest
    {
        [DataMember]
        public long Request_ID { get; set; }
        [DataMember]
        public string Requested_By { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public Group Group { get; set; }
        [DataMember]
        public string MajorGroup_Id { get; set; }
        [DataMember]
        public UserInfo User { get; set; }    
        [DataMember]
        public string Org_Id { get; set; }
        [DataMember]
        public bool? Active { get; set; }
    }
}
