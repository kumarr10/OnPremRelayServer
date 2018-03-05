using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class Department
    {
        [DataMember]
        public string Dept_Id { get; set; }
        [DataMember]
        public string Dept_Name { get; set; }
        [DataMember]
        public Section[] Sections { get; set; }
    }
}
