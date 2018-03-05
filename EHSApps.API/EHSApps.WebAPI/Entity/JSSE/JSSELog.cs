using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class JSSELog
    {
        [DataMember]
        public int Log_Id { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Stacktrace { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string EventCategory { get; set; }
        [DataMember]
        public string EventType { get; set; }
        [DataMember]
        public string EventFunction { get; set; }      
        [DataMember]
        public string AppURL { get; set; }
    }
}
