using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class UserGroup
    {
        [DataMember]
        public int UserGroup_ID { get; set; }
        [DataMember]
        public UserInfo User { get; set; }
        [DataMember]
        public UserInfo[] Users { get; set; }
        [DataMember]
        public Group Group { get; set; }
        [DataMember]
        public Hierarchy UserHierarchy { get; set; }
        [DataMember]
        public Group[] Groups { get; set; }
        [DataMember]
        public string ModifiedBy { get; set; }
        [DataMember]
        public DateTime? ModifiedDate { get; set; }
    }
}
