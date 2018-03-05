using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class JSSEAttachment
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string FileName { get; set; }

        //[DataMember]
        //public HttpPostedFileBase Image { get; set; }

        //[DataMember]
        //public byte[] Image { get; set; } 

        [DataMember]
        public string Base64ImageString { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }
    }
}
