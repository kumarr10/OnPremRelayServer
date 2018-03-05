using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class SecurityRequest
    {
        [DataMember]
        public string Org_Id { get; set; }
        [DataMember]
        public string Org_Name { get; set; }
        [DataMember]
        public string EmailBody { get; set; }             
        [DataMember]
        public string UserName { get; set; }      
    }
}
