using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class MasterCategory
    {
        [DataMember]
        public int Category_ID { get; set; }
        [DataMember]
        public int RowID { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string CategoryDesc { get; set; }
        [DataMember]
        public int? SortOrder { get; set; }
        [DataMember]
        public MasterBehavior[] Behaviors { get; set; }
        [DataMember]
        public string ModifiedBy { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public bool IsRequired { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string IsRequiredText { get;  set; }
    }
}
