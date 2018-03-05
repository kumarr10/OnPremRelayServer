using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class Hierarchy
    {
        [DataMember]
        public string MajorGroup_Id { get; set; }
        [DataMember]
        public string Org_Id { get; set; }
        [DataMember]
        public string MajorGroup { get; set; }
        [DataMember]
        public string Organization { get; set; }
        [DataMember]
        public string Dept_Id { get; set; }
        [DataMember]
        public string Section_Id { get; set; }
        [DataMember]
        public string Department { get; set; }
        [DataMember]
        public string Section { get; set; }
    }
}
