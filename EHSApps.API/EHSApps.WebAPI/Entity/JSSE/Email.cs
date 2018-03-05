using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class Email
    {
        [DataMember]
        public string EmailType { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string ToAddress { get; set; }       
        [DataMember]
        public string FromAddress { get; set; }
        [DataMember]
        public string CCAddress { get; set; }
        [DataMember]
        public string Body { get; set; }
        [DataMember]
        public bool? Active { get; set; }
    }
}
