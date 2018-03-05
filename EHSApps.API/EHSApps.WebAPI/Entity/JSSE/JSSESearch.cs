using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class JSSESearch
    {
        /// <summary>
        /// Organization Id, manadatory for search
        /// </summary>
        [DataMember]
        public int? Org_Id { get; set; }
        /// <summary>
        /// groupType has to be 2 for Owner Organization
        /// </summary>
        [DataMember]
        public int GroupType { get; set; }
        /// <summary>
        /// owner orgs for current logged in user
        /// </summary>
        [DataMember]
        public string OwnerOrgIds { get; set; }
        /// <summary>
        /// from date to search JSSEs
        /// </summary>
        [DataMember]
        public string FromDate { get; set; }
        /// <summary>
        /// to date to search JSSEs
        /// </summary>
        [DataMember]
        public string ToDate { get; set; }
        /// <summary>
        /// logged in user name without domain
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
    }
}
