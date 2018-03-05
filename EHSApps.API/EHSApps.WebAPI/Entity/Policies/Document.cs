using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.WebAPI.PolicyProcedure.Entity
{
    [DataContract]
    public class Document
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string DocumentID { get; set; }
        [DataMember]
        public string DocumentName { get; set; }
        [DataMember]
        public string DocSubType { get; set; }
        [DataMember]
        public string DocType { get; set; }
        [DataMember]
        public string DocumentPath { get; set; }
        [DataMember]
        public string Modified { get; set; }
        [DataMember]
        public string ModifiedBy { get; set; }
        [DataMember]
        public string Category { get;  set; }
    }
}
