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
    
    public partial class T_JSSE_Log
    {
        public long Log_ID { get; set; }
        public string EventType { get; set; }
        public string EventCategory { get; set; }
        public string EventFunction { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string AppURL { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}