using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class Permission
    {
        [DataMember]
        public int Permission_ID { get; set; }
        [DataMember]
        public string PermissionName { get; set; }
        [DataMember]
        public string PermissionDesc { get; set; }
        [DataMember]
        public bool Selected { get; set; }
        [DataMember]
        public bool Disabled { get; set; }
        [DataMember]
        public GroupType PermissionLevel { get; set; }
    }
    [DataContract]
    public class GroupType
    {
        [DataMember]
        public int? Level_Id { get; set; }
        [DataMember]
        public string Level_Name { get; set; }
        [DataMember]
        public bool Selected { get; set; }
         [DataMember]
        public string LevelDesc { get; set; }
    }
}
