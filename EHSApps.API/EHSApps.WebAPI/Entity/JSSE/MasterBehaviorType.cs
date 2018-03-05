using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class MasterBehaviorType
    {
        [DataMember]
        public int BehaviorType_ID { get; set; }
        [DataMember]
        public string BehaviorType { get; set; }
    }
}