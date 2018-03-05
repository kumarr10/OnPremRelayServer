using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class MasterBehavior
    {
        [DataMember]
        public int Behavior_ID { get; set; }
        [DataMember]
        public string Behavior { get; set; }
        [DataMember]
        public string BehaviorDesc { get; set; }
        [DataMember]
        public int? MajorGroup_ID { get; set; }
        [DataMember]
        public int? Org_ID { get; set; }
        [DataMember]
        public MasterBehaviorType BehaviorType { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public MasterCategory Category { get; set; }
        [DataMember]
        public string ModifiedBy { get; set; }
        [DataMember]
        public int? SortOrder { get;  set; }
        [DataMember]
        public int RowID { get;  set; }
    }
}