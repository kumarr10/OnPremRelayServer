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
    
    public partial class vw_JSSE_GetGroupRequests
    {
        public long Request_ID { get; set; }
        public int SecurityGroup_ID { get; set; }
        public string GroupName { get; set; }
        public string MajorGroup_Id { get; set; }
        public string Org_Id { get; set; }
        public int Emp_No { get; set; }
        public int Company_Cd { get; set; }
        public string EMAIL_ADDRESS_COMPANY { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PRIMARY_WINDOWS_NT_ACCOUNT { get; set; }
        public Nullable<short> User_MajorGroup_Id { get; set; }
        public Nullable<short> User_Org_Id { get; set; }
        public Nullable<short> User_Dept_Id { get; set; }
        public Nullable<short> Use_Sect_Id { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}
