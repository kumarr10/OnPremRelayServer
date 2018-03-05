using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class Comment
    {
        [DataMember]
        public int Comment_ID { get; set; }
        [DataMember]
        public string CommentName { get; set; }
    }
}
