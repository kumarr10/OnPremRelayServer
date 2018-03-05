using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class Group
    {
        [DataMember]
        public int Group_ID { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public string ModifiedBy { get; set; }
        [DataMember]
        public string GroupDesc { get; set; }
        [DataMember]
        public DateTime? ModifiedDate { get; set; }
        [DataMember]
        public GroupType GroupType { get; set; }
        [DataMember]
        public string MajorGroup_Id { get; set; }
        [DataMember]
        public string Org_Id { get; set; }
        [DataMember]
        public Permission[] Permissions { get; set; }
        [DataMember]
        public bool? Active { get; set; }
        [DataMember]
        public string Status { get; set; }
    }
}
