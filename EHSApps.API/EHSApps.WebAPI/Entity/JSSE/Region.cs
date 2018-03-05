using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class Region
    {
        [DataMember]
        public int Region_ID { get; set; }
        [DataMember]
        public string RegionName { get; set; }
    }
}
