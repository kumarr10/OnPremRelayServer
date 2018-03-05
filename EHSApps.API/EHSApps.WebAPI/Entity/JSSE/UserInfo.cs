using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class UserInfo
    {
        [DataMember]
        public int? CompanyId { get; set; }
        [DataMember]
        public string UserRole { get; set; }
        [DataMember]
        public long Emp_Id { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string UserTitle { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string User_ID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MajorGroup_Id { get; set; }
        [DataMember]
        public string Org_Id { get; set; }
        [DataMember]
        public string Dept_Id { get; set; }
        [DataMember]
        public string Section_Id { get; set; }
        [DataMember]
        public int SecurityUserID { get; set; }
    }

    public enum JSSERole
    {
        Observer = 1,
        Supervisor = 2
    }
}
