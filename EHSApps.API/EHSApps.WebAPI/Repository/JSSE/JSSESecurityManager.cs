using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using EHSApps.API.JSSE.Entity;
using EHSApps.API.JSSE.Data;
using System.Data.Entity.Core.EntityClient;
using ConEdison.EHSApps.WebAPI.Models;
using EHSApps.API.Utils;

namespace Coned.EHSApps.JSSE.Library.Data
{
    public class JSSESecurityManager
    {
        public static IEnumerable<EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS> GetUserOrganizations(string user_Id, int groupType)
        {
            var context = Global.Context;
            var jSSEResults = groupType == 1 ? from c in context.EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS
                                               join g in context.T_JSSE_Security_Group.Include("T_JSSE_Security_UserGroup").Include("T_JSSE_Security_UserGroup.T_JSSE_Security_User") on c.org_cd equals g.Org_Id
                                               select c :
                                               from c in context.EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS
                                               join g in context.T_JSSE_Security_Group.Include("T_JSSE_Security_UserGroup").Include("T_JSSE_Security_GroupPermission").Include("T_JSSE_Security_UserGroup.T_JSSE_Security_User") on c.org_cd equals g.Org_Id
                                               where g.T_JSSE_Security_UserGroup.Any(b => b.T_JSSE_Security_User.User_ID == user_Id)
                                               select c;

            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }

        public static IEnumerable<EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS> GetUserOrgsByPermission(string user_Id, int groupType, int permissionID)
        {
            var context = Global.Context;
            var jSSEResults = groupType == 1 ? from c in context.EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS
                                               join g in context.T_JSSE_Security_Group.Include("T_JSSE_Security_UserGroup").Include("T_JSSE_Security_UserGroup.T_JSSE_Security_User") on c.org_cd equals g.Org_Id
                                               select c :
                                               from c in context.EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS
                                               join g in context.T_JSSE_Security_Group.Include("T_JSSE_Security_UserGroup").Include("T_JSSE_Security_GroupPermission").Include("T_JSSE_Security_UserGroup.T_JSSE_Security_User") on c.org_cd equals g.Org_Id
                                               where g.T_JSSE_Security_UserGroup.Any(b => b.T_JSSE_Security_User.User_ID == user_Id) && g.T_JSSE_Security_GroupPermission.Any(s => s.T_JSSE_Security_Permission.SecurityPermission_ID == permissionID)
                                               select c;

            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }

        public static IEnumerable<EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS> GetObserveeOrgs(string user_Id, int groupType)
        {
            var context = Global.Context;
            var jSSEResults = from c in context.EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS
                              join g in context.EDW_COPY_vEMPLOYEE_INFO on c.org_cd equals Convert.ToString(g.ORG_CD)
                              where g.PRIMARY_WINDOWS_NT_ACCOUNT == user_Id
                              select c;
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }

        public static IEnumerable<EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS> GetOrgName(string orgId)
        {
            var jSSEResults = from c in Global.Context.EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS
                              where c.org_cd == orgId
                              select c;
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }

        public static IEnumerable<EDW_COPY_vEMPLOYEE_INFO> SearchOrgUsers(string firstName, string lastName, int org_Id)
        {
            var context = Global.Context;
            var jSSEResults = org_Id > 0 ? from c in context.EDW_COPY_vEMPLOYEE_INFO
                                           where c.ORG_CD == org_Id && c.ACTIVE == 1
                                           select c :
                                           from c in context.EDW_COPY_vEMPLOYEE_INFO
                                           where c.ACTIVE == 1
                                           select c;
            if (!string.IsNullOrEmpty(firstName))
                jSSEResults = jSSEResults.Where(s => s.FIRST_NAME.ToLower().StartsWith(firstName.ToLower()));
            if (!string.IsNullOrEmpty(lastName))
                jSSEResults = jSSEResults.Where(s => s.LAST_NAME.ToLower().StartsWith(lastName.ToLower()));
            var jSSEEnumerable = jSSEResults.OrderBy(s => s.FIRST_NAME).OrderBy(s => s.LAST_NAME).AsEnumerable();
            return jSSEEnumerable;
        }

        public static IEnumerable<EDW_COPY_vEMPLOYEE_INFO> SearchUsers(string firstName, string lastName)
        {
            var jSSEResults = from c in Global.Context.EDW_COPY_vEMPLOYEE_INFO where c.ACTIVE == 1 select c;
            if (!string.IsNullOrEmpty(firstName))
                jSSEResults = jSSEResults.Where(s => s.FIRST_NAME.ToLower().StartsWith(firstName.ToLower()));
            if (!string.IsNullOrEmpty(lastName))
                jSSEResults = jSSEResults.Where(s => s.LAST_NAME.ToLower().StartsWith(lastName.ToLower()));
            var jSSEEnumerable = jSSEResults.OrderBy(s => s.FIRST_NAME).OrderBy(s => s.LAST_NAME).AsEnumerable();
            return jSSEEnumerable;
        }

        public static int AddUser(UserGroup group)
        {
            int result = 0;
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(Global.jSSEConn);
            string HierInfo = group.User.MajorGroup_Id + "," + group.User.Org_Id + "," + group.User.Dept_Id + "," + group.User.Section_Id;
            //Parameter Names
            string[] JsseParams = new[] { "GroupID", "UserID", "FirstName", "LastName", "EmpNo", "CompanyId", "HierarchyInfo", "CreatedBy" };
            try
            {
                var JsseValues = new object[] { group.Group.Group_ID, group.User.User_ID, group.User.FirstName, group.User.LastName,
                                            group.User.Emp_Id, group.User.CompanyId, HierInfo, group.ModifiedBy};
                var jsseMain = DBGeneric.ExecStoredProcedure(entityBuilder.ProviderConnectionString, "dbo.[usp_JSSE_InsertUpdateGroupUsers]",
                                                JsseParams, JsseValues);
            }
            catch 
            {
                throw ;
            }
            return result;
        }
        public static int AddGroup(Group group)
        {
            int result = 0;
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(Global.jSSEConn);
            //Parameter Names
            string[] JsseParams = new[] { "GroupID", "GroupName", "GroupDesc", "GroupType", "MajorGroup_Id", "Org_Id", "PermissionIDs", "CreatedBy", "Active" };
            try
            {
                var JsseValues = new object[] { 0, group.GroupName, group.GroupDesc, group.GroupType.Level_Id, group.MajorGroup_Id,group.Org_Id,
                                            string.Join(",",group.Permissions.Where(p=>p.Selected).Select(p=>p.Permission_ID.ToString()).ToArray()), 
                                            group.ModifiedBy, 1};
                var jsseMain = DBGeneric.ExecStoredProcedure(entityBuilder.ProviderConnectionString, "dbo.[usp_JSSE_InsertUpdateSecurityGroup]",
                                                JsseParams, JsseValues);
            }
            catch 
            {
                throw ;
            }
            return result;
        }

        public static int UpdateGroup(Group group)
        {
            int result = 0;
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(Global.jSSEConn);
            //Parameter Names
            if (group.Active == null)
                group.Active = true;
            string[] JsseParams = new[] { "GroupID", "GroupName", "GroupDesc", "GroupType", "MajorGroup_Id", "Org_Id", "PermissionIDs", "CreatedBy", "Active" };
            try
            {
                var JsseValues = new object[] { group.Group_ID, group.GroupName, group.GroupDesc, group.GroupType.Level_Id, group.MajorGroup_Id,group.Org_Id,
                                                string.Join(",", group.Permissions.Where(p=>p.Selected).Select(p => p.Permission_ID.ToString()).ToArray()), 
                                               group.ModifiedBy, group.Active };
                var jsseMain = DBGeneric.ExecStoredProcedure(entityBuilder.ProviderConnectionString, "dbo.[usp_JSSE_InsertUpdateSecurityGroup]",
                                                JsseParams, JsseValues);
            }
            catch 
            {
                throw;
            }
            return result;
        }
        public static int RemoveUserFromGroup(int groupId, int DbUserId)
        {
            int result = 0;
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(Global.jSSEConn);
            //Parameter Names
            string[] JsseParams = new[] { "GroupId", "DbUserId" };
            try
            {
                var JsseValues = new object[] { groupId, DbUserId };
                var jsseMain = DBGeneric.ExecStoredProcedure(entityBuilder.ProviderConnectionString, "dbo.[usp_JSSE_DeleteUserFromGroup]",
                                                JsseParams, JsseValues);
            }
            catch 
            {
                throw ;
            }
            return result;
        }
        public static IEnumerable<vw_JSSE_GetSecurityGroups> GetSecurityGroups(int? levelID)
        {
            var jSSEResults = from c in Global.Context.vw_JSSE_GetSecurityGroups select c;
            if (levelID != null && levelID > 0)
                jSSEResults = jSSEResults.Where(x => x.Level_ID == levelID);
            //int totalGrps = 0;
            //var firstPageData = DBGeneric.PagedResult(jSSEResults, 1, 20, grp => grp.GroupName, false, out totalGrps);
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }
        public static IEnumerable<vw_JSSE_GetGroupUsers> GetGroupUsers(int groupId)
        {
            var jSSEResults = from c in Global.Context.vw_JSSE_GetGroupUsers where c.SecurityGroup_ID == groupId select c;
            //int totalGrps = 0;
            //var firstPageData = DBGeneric.PagedResult(jSSEResults, 1, 20, grp => grp.GroupName, false, out totalGrps);
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }
        public static IEnumerable<vw_JSSE_GetGroupRequests> GetGroupRequests(int groupId)
        {
            var jSSEResults = from c in Global.Context.vw_JSSE_GetGroupRequests where c.SecurityGroup_ID == groupId select c;
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }
        public static IEnumerable<T_JSSE_Security_Permission> GetPermissions()
        {
            var jSSEResults = from c in Global.Context.T_JSSE_Security_Permission.Include("T_JSSE_Security_Level") select c;
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }
        public static IEnumerable<T_JSSE_Security_Group> GetGroupsByLevel(int levelId, string orgIds, int permissionID)
        {
            var orgIDList = orgIds.Split(',');
            var jSSEResults = from c in Global.Context.T_JSSE_Security_Group.Include("T_JSSE_Security_GroupPermission").Include("T_JSSE_Security_GroupPermission.T_JSSE_Security_Permission")
                              where c.GroupLevel == levelId
                              select c;
            if (permissionID > 0)
                jSSEResults = jSSEResults.Where(g => g.T_JSSE_Security_GroupPermission.Any(a => a.T_JSSE_Security_Permission.SecurityPermission_ID == permissionID));
            if (!string.IsNullOrEmpty(orgIds))
            {
                jSSEResults = jSSEResults.Where(DBGeneric.BuildContainsExpression<T_JSSE_Security_Group, string>(e => e.Org_Id, orgIDList));
            }
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }

        public static IEnumerable<T_JSSE_Security_Level> GetGroupTypes()
        {
            var jSSEResults = from c in Global.Context.T_JSSE_Security_Level select c;
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }
        public static EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS GetHierachyBySection(string sectionId)
        {
            var jSSEResults = from c in Global.Context.EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS where c.sect_cd == sectionId select c;
            var jSSEEnumerable = jSSEResults.FirstOrDefault();
            return jSSEEnumerable;
        }
        public static T_JSSE_Security_User GetUserSecurity(string userName)
        {
            try
            {
                var context = Global.Context;
                var empInfo = context.EDW_COPY_vEMPLOYEE_INFO.Where(e => e.PRIMARY_WINDOWS_NT_ACCOUNT == userName && e.ACTIVE == 1).FirstOrDefault();
                string empNo = empInfo.Emp_No.ToString();
                var jSSEResults = (from c in context.T_JSSE_Security_User.Include("T_JSSE_Security_UserGroup").Include("T_JSSE_Security_UserGroup.T_JSSE_Security_Group")
                                                                        .Include("T_JSSE_Security_UserGroup.T_JSSE_Security_Group.T_JSSE_Security_GroupPermission")
                                                                         .Include("T_JSSE_Security_UserGroup.T_JSSE_Security_Group.T_JSSE_Security_GroupPermission.T_JSSE_Security_Permission")
                                                                        where c.User_ID == userName && c.EmptNo == empNo
                                                                        select c).FirstOrDefault();
                return jSSEResults;
            }
            catch 
            {
                throw new Exception("User does not exist in Employee Database. Please check if you are using system accounts.");
            }
        }
        public static EDW_COPY_vEMPLOYEE_INFO GetUserInfo(string userName)
        {
            if (!userName.ToLower().StartsWith("coned"))
                userName = "coned\\" + userName;
            var empInfo = Global.Context.EDW_COPY_vEMPLOYEE_INFO.Where(e => e.PRIMARY_WINDOWS_NT_ACCOUNT == userName && e.ACTIVE == 1).FirstOrDefault();
            return empInfo;
        }

        public static int AddUserRequest(T_JSSE_Security_Request request, string orgId)
        {
            int result = 0;
            var context = Global.Context;
            var dbRequest = context.T_JSSE_Security_Request.Include("T_JSSE_Security_Group").Where(d => d.Requested_By == request.Requested_By && d.T_JSSE_Security_Group.Org_Id == orgId);
            var usrGrp = context.T_JSSE_Security_Group.First(d => (d.Org_Id == orgId && d.GroupLevel == 3));
            if (usrGrp == null)
                result = -2; // -2 for User Group does not exist in Database for selected Organization   
            else if (dbRequest.Count() > 0)
                result = -1; // User Request has been already created and Saved in database
            else
            {
                //Global.Context.AddToT_JSSE_Security_Request(request);
                //Global.Context.Attach(usrGrp);
                request.T_JSSE_Security_Group = usrGrp;
                result = Global.Context.SaveChanges();
            }
            return result;
        }
        public static int DeActiveUserRequest(long requestId)
        {
            int result = 0;
            var context = Global.Context;
            if (requestId > 0)
            {
                var requestInfo = context.T_JSSE_Security_Request.First(d => d.Request_ID == requestId);
                if (requestInfo != null)
                {
                    requestInfo.Active = false;
                    result = context.SaveChanges();
                }
                else
                    result = -1; // Request ID does not exist in DB
            }
            return result;
        }
        public static int RemoveUserRequest(long requestId)
        {
            var context = Global.Context;
            int result = 0;
            if (requestId > 0)
            {
                var requestInfo = context.T_JSSE_Security_Request.First(d => d.Request_ID == requestId);
                if (requestInfo != null)
                {
                    //Global.Context.DeleteObject(requestInfo);
                    result = context.SaveChanges();
                }
                else
                    result = -1; // Request ID does not exist in DB
            }
            return result;
        }
    }
}
