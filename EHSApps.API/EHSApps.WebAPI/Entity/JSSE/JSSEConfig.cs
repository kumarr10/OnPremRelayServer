using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class JSSEConfig
    {
        [DataMember]
        public string ConfigType { get; set; }
        [DataMember]
        public Hashtable ConfigEntry { get; set; }      
        [DataMember]
        public bool Active { get; set; }
    }
}
