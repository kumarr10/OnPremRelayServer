using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class ContractCompany
    {
        [DataMember]
        public int ContractCompany_Id { get; set; }
        [DataMember]
        public string ContractCompany_Name { get; set; }   
        [DataMember]
        public int? SortOrder { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string ModifiedBy { get; set; }
        [DataMember]
        public DateTime? ModifiedDate { get; set; }
        [DataMember]
        public bool IsRequired { get; set; }      
    }
}
