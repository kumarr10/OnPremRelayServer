using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class BehaviorFilter
    {
        [DataMember]
        public int CategoryId { get; set; }
        [DataMember]
        public int BehaviorTypeId { get; set; }
        [DataMember]
        public int MajorGroupId { get; set; }
        [DataMember]
        public int OrgId { get; set; }     
    }
}
