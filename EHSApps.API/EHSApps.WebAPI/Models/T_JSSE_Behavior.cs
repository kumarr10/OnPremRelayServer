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
    
    public partial class T_JSSE_Behavior
    {
        public int JSSEBehavior_ID { get; set; }
        public Nullable<int> JSSECategory_ID { get; set; }
        public Nullable<int> Behavior_ID { get; set; }
        public Nullable<int> BehaviorType_ID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Rating_ID { get; set; }
        public string Comment { get; set; }
    
        public virtual T_JSSE_Category T_JSSE_Category { get; set; }
        public virtual T_JSSE_Master_Behavior T_JSSE_Master_Behavior { get; set; }
        public virtual T_JSSE_Master_BehaviorType T_JSSE_Master_BehaviorType { get; set; }
        public virtual T_JSSE_Master_Rating T_JSSE_Master_Rating { get; set; }
    }
}