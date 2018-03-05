using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class Section
    {
        [DataMember]
        public string Section_Id { get; set; }
        [DataMember]
        public string Section_Name { get; set; }
    }
}
