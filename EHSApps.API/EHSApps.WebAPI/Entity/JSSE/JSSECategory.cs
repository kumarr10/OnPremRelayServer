using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class JSSECategory
    {
        [DataMember]
        public int Category_ID { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string CategoryDesc { get; set; }
        [DataMember]
        public int? SortOrder { get; set; }
        [DataMember]
        public JSSEBehavior[] EntBehaviors { get; set; }
        [DataMember]
        public JSSEBehavior[] OrgBehaviors { get; set; }
        [DataMember]
        public JSSERating[] Ratings { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public string RatingID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsRequired { get; set; }
        [DataMember]
        public long? JSSE_ID { get; set; }
        [DataMember]
        public List<JSSEBehavior[]> AllOrgBehaviors { get; set; }
    }
}
