using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class JSSEBehavior
    {
        [DataMember]
        public int Behavior_ID { get; set; }
        [DataMember]
        public string Behavior { get; set; }
        [DataMember]
        public string BehaviorDesc { get; set; }
        [DataMember]
        public int BehaviorType_ID { get; set; }
        [DataMember]
        public string BehaviorType { get; set; }
        [DataMember]
        public int? Org_ID { get; set; }
        [DataMember]
        public bool BehviorChecked { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int? Category_ID { get; set; }
        [DataMember]
        public int? Rating_ID { get; set; }
        [DataMember]
        public string Rating { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public int? SortOrder { get; set; }
    }
}
