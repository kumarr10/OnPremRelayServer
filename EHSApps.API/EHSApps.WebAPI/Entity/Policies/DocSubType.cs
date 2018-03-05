using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.WebAPI.PolicyProcedure.Entity
{
    [DataContract]
    public class DocSubType
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Document[] Documents { get; set; }

    }
}
