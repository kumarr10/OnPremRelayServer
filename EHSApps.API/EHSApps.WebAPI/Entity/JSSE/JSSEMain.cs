using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class JSSEMain
    {
        [DataMember]
        public Int64 JSSE_ID { get; set; }
        [DataMember]
        public int Observer_ID { get; set; }
        [DataMember]
        public int Observee_ID { get; set; }
        [DataMember]
        public int Hierarchy_ID { get; set; }
        [DataMember]
        public int? Region_ID { get; set; }
        [DataMember]
        public string Region { get; set; }
        [DataMember]
        public string JSSETitle { get; set; }
        [DataMember]
        public string JobName { get; set; }
        [DataMember]
        public string JobDescription { get; set; }
        [DataMember]
        public DateTime? JSSEDate { get; set; }
        [DataMember]
        public JSSECategory[] Categories { get; set; }
        [DataMember]
        public string JSSEStatus { get; set; }
        [DataMember]
        public bool? IsAnonymous { get; set; }
        [DataMember]
        public bool? IsOBSRAnonymous { get; set; }
        [DataMember]
        public int? MajorGroup_Id { get; set; }
        [DataMember]
        public int? Org_Id { get; set; }
        [DataMember]
        public int? Dept_Id { get; set; }
        [DataMember]
        public int? Section_Id { get; set; }
        [DataMember]
        public string JSSEEnteredBy { get; set; }
        [DataMember]
        public string JSSECreator { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public UserInfo Observer { get; set; }
        [DataMember]
        public UserInfo[] Observers{ get; set; }
        [DataMember]
        public int IsSupervsrOBSRSame { get; set; }
        [DataMember]
        public UserInfo[] Supervisors { get; set; }
        [DataMember]
        public UserInfo[] Observees { get; set; }
        [DataMember]
        public string SelObservers { get; set; }
        [DataMember]
        public string SelObservees { get; set; }
        [DataMember]
        public string SelSupervisors { get; set; }
        [DataMember]
        public bool? IsExternal { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public JSSEAttachment[] Attachments { get; set; }
        [DataMember]
        public bool IsCreator { get; set; }
        [DataMember]
        public string Base64_JSSE_ID { get; set; }
        [DataMember]
        public MajorGroup[] majorgroups { get; set; }
        [DataMember]
        public Organization[] organizations { get; set; }
        [DataMember]
        public Department[] departments { get; set; }
        [DataMember]
        public Section[] sections { get; set; }
        [DataMember]
        public int? AttachmentCount { get; set; }
        [DataMember]
        public string Image { get; set; }
        [DataMember]
        public string ImageName { get; set; }
    }
}
