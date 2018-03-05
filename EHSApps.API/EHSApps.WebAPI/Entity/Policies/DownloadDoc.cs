using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.WebAPI.PolicyProcedure.Entity
{
    [DataContract]
    public class DownloadDoc
    {
        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
