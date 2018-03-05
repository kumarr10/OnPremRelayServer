using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class Organization
    {
        [DataMember]
        public string Org_Id { get; set; }
        [DataMember]
        public string Org_Name { get; set; }
        [DataMember]
        public Department[] Departments { get; set; }
        [DataMember]
        public string MajorGroup_Id { get; set; }
        [DataMember]
        public string MajorGroup_Name { get; set; }
    }
}
