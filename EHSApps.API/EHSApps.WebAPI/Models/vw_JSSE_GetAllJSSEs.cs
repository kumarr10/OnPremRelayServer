//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConEdison.EHSApps.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_JSSE_GetAllJSSEs
    {
        public long JSSE_ID { get; set; }
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        public Nullable<int> Region_ID { get; set; }
        public string RegionName { get; set; }
        public Nullable<bool> IsAnonymous { get; set; }
        public Nullable<bool> IsExternal { get; set; }
        public Nullable<System.DateTime> JSSEDate { get; set; }
        public string Status { get; set; }
        public string Observees { get; set; }
        public string Observer { get; set; }
        public string ObserverUserID { get; set; }
        public Nullable<int> MajorGroup_Id { get; set; }
        public Nullable<int> Org_Id { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Location { get; set; }
        public Nullable<int> AttachmentCount { get; set; }
    }
}