using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class SMTPSettings
    {
        [DataMember]
        public string SMTPServer { get; set; }
        [DataMember]
        public string FromAddress { get; set; }     
    }
}
