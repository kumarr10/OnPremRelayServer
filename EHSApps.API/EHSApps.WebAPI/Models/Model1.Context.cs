﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class JSSEEntityConn : DbContext
    {
        public JSSEEntityConn()
            : base("name=JSSEEntityConn")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<EDW_COPY_vEMPLOYEE_INFO> EDW_COPY_vEMPLOYEE_INFO { get; set; }
        public virtual DbSet<EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS> EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS { get; set; }
        public virtual DbSet<T_JSSE_Attachments> T_JSSE_Attachments { get; set; }
        public virtual DbSet<T_JSSE_Audit_Behavior> T_JSSE_Audit_Behavior { get; set; }
        public virtual DbSet<T_JSSE_Audit_Category> T_JSSE_Audit_Category { get; set; }
        public virtual DbSet<T_JSSE_Audit_CategoryBehavior_Map> T_JSSE_Audit_CategoryBehavior_Map { get; set; }
        public virtual DbSet<T_JSSE_Audit_Goal> T_JSSE_Audit_Goal { get; set; }
        public virtual DbSet<T_JSSE_Audit_Security> T_JSSE_Audit_Security { get; set; }
        public virtual DbSet<T_JSSE_Audit_User> T_JSSE_Audit_User { get; set; }
        public virtual DbSet<T_JSSE_Behavior> T_JSSE_Behavior { get; set; }
        public virtual DbSet<T_JSSE_Category> T_JSSE_Category { get; set; }
        public virtual DbSet<T_JSSE_Comment> T_JSSE_Comment { get; set; }
        public virtual DbSet<T_JSSE_Configuration> T_JSSE_Configuration { get; set; }
        public virtual DbSet<T_JSSE_EDW_EmpInfo_Sync> T_JSSE_EDW_EmpInfo_Sync { get; set; }
        public virtual DbSet<T_JSSE_Goal> T_JSSE_Goal { get; set; }
        public virtual DbSet<T_JSSE_Goal_Frequency> T_JSSE_Goal_Frequency { get; set; }
        public virtual DbSet<T_JSSE_Goal_Level> T_JSSE_Goal_Level { get; set; }
        public virtual DbSet<T_JSSE_Hierarchy> T_JSSE_Hierarchy { get; set; }
        public virtual DbSet<T_JSSE_Log> T_JSSE_Log { get; set; }
        public virtual DbSet<T_JSSE_Main> T_JSSE_Main { get; set; }
        public virtual DbSet<T_JSSE_Master_Behavior> T_JSSE_Master_Behavior { get; set; }
        public virtual DbSet<T_JSSE_Master_BehaviorType> T_JSSE_Master_BehaviorType { get; set; }
        public virtual DbSet<T_JSSE_Master_Category> T_JSSE_Master_Category { get; set; }
        public virtual DbSet<T_JSSE_Master_CategoryBehavior_Map> T_JSSE_Master_CategoryBehavior_Map { get; set; }
        public virtual DbSet<T_JSSE_Master_Rating> T_JSSE_Master_Rating { get; set; }
        public virtual DbSet<T_JSSE_Master_Region> T_JSSE_Master_Region { get; set; }
        public virtual DbSet<T_JSSE_Observee> T_JSSE_Observee { get; set; }
        public virtual DbSet<T_JSSE_Observer> T_JSSE_Observer { get; set; }
        public virtual DbSet<T_JSSE_Report_SaveTemplate> T_JSSE_Report_SaveTemplate { get; set; }
        public virtual DbSet<T_JSSE_Report_Subscription> T_JSSE_Report_Subscription { get; set; }
        public virtual DbSet<T_JSSE_Security_Group> T_JSSE_Security_Group { get; set; }
        public virtual DbSet<T_JSSE_Security_GroupPermission> T_JSSE_Security_GroupPermission { get; set; }
        public virtual DbSet<T_JSSE_Security_Level> T_JSSE_Security_Level { get; set; }
        public virtual DbSet<T_JSSE_Security_Permission> T_JSSE_Security_Permission { get; set; }
        public virtual DbSet<T_JSSE_Security_Request> T_JSSE_Security_Request { get; set; }
        public virtual DbSet<T_JSSE_Security_User> T_JSSE_Security_User { get; set; }
        public virtual DbSet<T_JSSE_Security_UserGroup> T_JSSE_Security_UserGroup { get; set; }
        public virtual DbSet<vw_JSSE_GetAllJSSEs> vw_JSSE_GetAllJSSEs { get; set; }
        public virtual DbSet<vw_JSSE_GetGroupRequests> vw_JSSE_GetGroupRequests { get; set; }
        public virtual DbSet<vw_JSSE_GetGroupUsers> vw_JSSE_GetGroupUsers { get; set; }
        public virtual DbSet<vw_JSSE_GetSecurityGroups> vw_JSSE_GetSecurityGroups { get; set; }
    }
}