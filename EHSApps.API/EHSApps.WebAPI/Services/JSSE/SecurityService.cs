using Entities = EHSApps.API.JSSE.Entity;
using System;
using System.Collections.Generic;
using Coned.EHSApps.JSSE.Library.Data;
using System.Linq;
using System.Text;
using ConEdison.EHSApps.WebAPI.Models;
using EHSApps.API.JSSE.Utils;
using ConEdison.EHSApps.WebAPI.interfaces;

namespace EHSApps.API.JSSE.Services
{
    public class SecurityService : ISecurityService
    {
        /* Get Group Users of selected */
        public  Entities.UserGroup GetUserSecurity(string userName)
        {
            if (!userName.ToLower().StartsWith("coned"))
                userName = "coned\\" + userName;
            Entities.UserGroup userGroup = new Entities.UserGroup();
            try
            {
                var dbUserGroup = JSSESecurityManager.GetUserSecurity(userName);
                if (dbUserGroup != null)
                {
                    userGroup.User = new Entities.UserInfo();
                    userGroup.User.SecurityUserID = dbUserGroup.SecurityUser_ID;
                    userGroup.User.User_ID = dbUserGroup.User_ID;
                    int empId = 0;
                    Int32.TryParse(dbUserGroup.EmptNo, out empId);
                    userGroup.User.Emp_Id = empId;
                    userGroup.User.FirstName = dbUserGroup.FirstName;
                    userGroup.User.LastName = dbUserGroup.LastName;
                    userGroup.User.FullName = dbUserGroup.LastName + " , " + dbUserGroup.FirstName;
                    userGroup.User.Email = dbUserGroup.Email;
                    userGroup.User.User_ID = dbUserGroup.User_ID;
                    userGroup.User.Org_Id = dbUserGroup.Org_ID;
                    userGroup.User.MajorGroup_Id = dbUserGroup.MajorGroup_ID;
                    userGroup.User.Dept_Id = dbUserGroup.Dept_Id;
                    userGroup.User.Section_Id = dbUserGroup.SectionId;

                    List<Entities.Group> groups = new List<Entities.Group>();
                    foreach (var ugp in dbUserGroup.T_JSSE_Security_UserGroup)
                    {
                        Entities.Group group = new Entities.Group();
                        group.Group_ID = ugp.T_JSSE_Security_Group.SecurityGroup_ID;
                        group.GroupName = ugp.T_JSSE_Security_Group.GroupName;
                        group.GroupType = new Entities.GroupType() { Level_Id = ugp.T_JSSE_Security_Group.GroupLevel };
                        group.MajorGroup_Id = ugp.T_JSSE_Security_Group.MajorGroup_Id;
                        group.Org_Id = ugp.T_JSSE_Security_Group.Org_Id;
                        List<Entities.Permission> perms = new List<Entities.Permission>();
                        foreach (var dbPerm in ugp.T_JSSE_Security_Group.T_JSSE_Security_GroupPermission)
                        {
                            Entities.Permission perm = new Entities.Permission();
                            perm.Permission_ID = dbPerm.T_JSSE_Security_Permission.SecurityPermission_ID;
                            perm.PermissionName = dbPerm.T_JSSE_Security_Permission.PermissionName;
                            perms.Add(perm);
                        }
                        group.Permissions = perms.ToArray();
                        groups.Add(group);
                    }
                    userGroup.Groups = groups.ToArray();
                }
            }
            catch 
            {
                throw;
            }
            return userGroup;
        }
        /* Get User Info of a user by alias */
        public Entities.UserInfo GetUserInfo(string userName)
        {
            Entities.UserInfo userInfo = new Entities.UserInfo();
            try
            {
                var dbUserGroup = JSSESecurityManager.GetUserInfo(userName);
                if (dbUserGroup != null)
                {
                    userInfo.CompanyId = dbUserGroup.Company_Cd;
                    userInfo.Emp_Id = dbUserGroup.Emp_No;
                    userInfo.User_ID = dbUserGroup.PRIMARY_WINDOWS_NT_ACCOUNT;
                    userInfo.FirstName = dbUserGroup.FIRST_NAME;
                    userInfo.LastName = dbUserGroup.LAST_NAME;
                    userInfo.FullName = dbUserGroup.LAST_NAME + " " + dbUserGroup.FIRST_NAME;
                    userInfo.Email = dbUserGroup.EMAIL_ADDRESS_COMPANY;
                    userInfo.MajorGroup_Id = dbUserGroup.VP_CD.ToString();
                    userInfo.Org_Id = dbUserGroup.ORG_CD.ToString();
                    userInfo.Dept_Id = dbUserGroup.DEPT_CD.ToString();
                    userInfo.Section_Id = dbUserGroup.SECT_CD.ToString();
                }
            }
            catch 
            {
                throw;
            }
            return userInfo;
        }
        /* Add Security Request to add User to group */
        public int AddUserRequest(Entities.UserRequest request)
        {
            int result = 0;
            T_JSSE_Security_Request dbJSSE = new T_JSSE_Security_Request();
            dbJSSE.Requested_By = request.Requested_By;
            dbJSSE.Description = request.Description;
            dbJSSE.CreatedBy = request.Requested_By;
            dbJSSE.CreatedDate = DateTime.Now;
            dbJSSE.Active = true;
            result = JSSESecurityManager.AddUserRequest(dbJSSE, request.Org_Id);
            return result;
        }

        /* Remove Security Request to add User to group */
        public int RemoveUserRequest(Entities.UserRequest request)
        {
            int result = 0;
            result = JSSESecurityManager.RemoveUserRequest(request.Request_ID);
            return result;
        }

        /* Deactivate Security Request to add User to group */
        public int DeActivateUserRequest(Entities.UserRequest request)
        {
            int result = 0;
            result = JSSESecurityManager.DeActiveUserRequest(request.Request_ID);
            return result;
        }

        public List<Entities.UserGroup> GetGroupUsers(int groupId)
        {
            List<Entities.UserGroup> uGrps = new List<Entities.UserGroup>();
            try
            {
                var dbGroups = JSSESecurityManager.GetGroupUsers(groupId);
                foreach (var dbGrp in dbGroups)
                {
                    Entities.UserGroup uGrp = new Entities.UserGroup();
                    uGrp.Group = new Entities.Group();
                    uGrp.User = new Entities.UserInfo();
                    uGrp.User.SecurityUserID = dbGrp.SecurityUser_ID;
                    uGrp.User.User_ID = dbGrp.User_ID;
                    int empId = 0;
                    Int32.TryParse(dbGrp.EmptNo, out empId);
                    uGrp.User.Emp_Id = empId;
                    uGrp.User.CompanyId = dbGrp.CompanyCd;
                    uGrp.User.FirstName = dbGrp.FirstName;
                    uGrp.User.LastName = dbGrp.LastName;
                    uGrp.User.FullName = dbGrp.LastName + " " + dbGrp.FirstName;
                    uGrp.User.Email = dbGrp.Email;
                    uGrp.ModifiedDate = dbGrp.ModifiedDate;
                    uGrp.Group.Group_ID = dbGrp.SecurityGroup_ID;
                    uGrp.Group.GroupName = dbGrp.GroupName;
                    uGrp.Group.GroupType = new Entities.GroupType() { Level_Id = dbGrp.Level_ID, Level_Name = dbGrp.LevelName };
                    uGrp.Group.MajorGroup_Id = dbGrp.MajorGroup_Id;
                    uGrp.Group.Org_Id = dbGrp.Org_Id;
                    uGrps.Add(uGrp);
                }
            }
            catch 
            {
                throw;
            }
            return uGrps;
        }
        /* Get Group Requests of selected security group*/
        public List<Entities.UserRequest> GetGroupRequests(int groupId)
        {
            List<Entities.UserRequest> uReqs = new List<Entities.UserRequest>();
            try
            {
                var dbRequests = JSSESecurityManager.GetGroupRequests(groupId);
                foreach (var dbReq in dbRequests)
                {
                    Entities.UserRequest uReq = new Entities.UserRequest();
                    uReq.Group = new Entities.Group();
                    uReq.User = new Entities.UserInfo();
                    uReq.Request_ID = dbReq.Request_ID;
                    uReq.User.SecurityUserID = dbReq.SecurityGroup_ID;
                    uReq.User.User_ID = dbReq.PRIMARY_WINDOWS_NT_ACCOUNT;
                    uReq.User.Emp_Id = dbReq.Emp_No;
                    uReq.User.CompanyId = dbReq.Company_Cd;
                    uReq.User.FirstName = dbReq.FirstName;
                    uReq.User.LastName = dbReq.LastName;
                    uReq.User.FullName = dbReq.LastName + " " + dbReq.FirstName;
                    uReq.User.Email = dbReq.EMAIL_ADDRESS_COMPANY;
                    uReq.User.MajorGroup_Id = dbReq.User_MajorGroup_Id.ToString();
                    uReq.User.Org_Id = dbReq.User_Org_Id.ToString();
                    uReq.User.Dept_Id = dbReq.User_Dept_Id.ToString();
                    uReq.User.Section_Id = dbReq.Use_Sect_Id.ToString();
                    uReq.Description = dbReq.Description;
                    uReq.Requested_By = dbReq.CreatedBy;
                    uReq.CreatedDate = dbReq.CreatedDate;
                    uReq.Group.Group_ID = dbReq.SecurityGroup_ID;
                    uReq.Group.GroupName = dbReq.GroupName;
                    uReq.Group.MajorGroup_Id = dbReq.MajorGroup_Id;
                    uReq.Group.Org_Id = dbReq.Org_Id;
                    uReqs.Add(uReq);
                }
            }
            catch 
            {
                throw;
            }
            return uReqs;
        }
        public List<Entities.Group> GetSecurityGroups(int? levelID)
        {
            List<Entities.Group> groups = new List<Entities.Group>();
            try
            {
                var dbGroups = JSSESecurityManager.GetSecurityGroups(levelID);
                foreach (var dbGrp in dbGroups)
                {
                    Entities.Group grp = new Entities.Group();
                    grp.Group_ID = dbGrp.SecurityGroup_ID;
                    grp.GroupName = dbGrp.GroupName;
                    grp.GroupDesc = dbGrp.GroupDesc;
                    grp.GroupType = new Entities.GroupType() { Level_Id = dbGrp.Level_ID, Level_Name = dbGrp.LevelName };
                    grp.MajorGroup_Id = dbGrp.MajorGroup_Id;
                    grp.Org_Id = dbGrp.Org_Id;
                    grp.ModifiedDate = dbGrp.ModifiedDate;
                    if (dbGrp.PermIDList != null)
                    {
                        List<Entities.Permission> perms = new List<Entities.Permission>();
                        string[] permIDList = dbGrp.PermIDList.Split(',');
                        string[] permNameList = dbGrp.PermList.Split(',');
                        for (int dbPerm = 0; dbPerm < permIDList.Length; dbPerm++)
                        {
                            Entities.Permission perm = new Entities.Permission();
                            perm.Permission_ID = Int32.Parse(permIDList[dbPerm]);
                            perm.PermissionName = permNameList[dbPerm];
                            perms.Add(perm);
                        }
                        grp.Permissions = perms.ToArray();
                    }
                    groups.Add(grp);
                }
            }
            catch 
            {
                throw;
            }
            return groups;
        }
        public List<Entities.Group> GetGroupsByLevel(int levelId, string orgId, int permissionID)
        {
            List<Entities.Group> groups = new List<Entities.Group>();
            try
            {
                var dbGroups = JSSESecurityManager.GetGroupsByLevel(levelId, orgId, permissionID);
                foreach (var dbGrp in dbGroups)
                {
                    Entities.Group grp = new Entities.Group();
                    grp.Group_ID = dbGrp.SecurityGroup_ID;
                    grp.GroupName = dbGrp.GroupName;
                    grp.GroupDesc = dbGrp.GroupDesc;
                    grp.GroupType = new Entities.GroupType() { Level_Id = dbGrp.GroupLevel };
                    grp.MajorGroup_Id = dbGrp.MajorGroup_Id;
                    grp.Org_Id = dbGrp.Org_Id;
                    grp.ModifiedDate = dbGrp.ModifiedDate;
                    if (dbGrp.T_JSSE_Security_GroupPermission != null)
                    {
                        List<Entities.Permission> perms = new List<Entities.Permission>();
                        foreach (var dbPerm in dbGrp.T_JSSE_Security_GroupPermission)
                        {
                            Entities.Permission perm = new Entities.Permission();
                            perm.Permission_ID = dbPerm.T_JSSE_Security_Permission.SecurityPermission_ID;
                            perm.PermissionName = dbPerm.T_JSSE_Security_Permission.PermissionName;
                            perms.Add(perm);
                        }
                        grp.Permissions = perms.ToArray();
                    }
                    groups.Add(grp);
                }
            }
            catch 
            {
                throw;
            }
            return groups;
        }

        public int SendRequestEmail(Entities.SecurityRequest accessRequest)
        {
            //string userName = HttpContext.Current.User.Identity.Name;
            if (accessRequest != null && !string.IsNullOrEmpty(accessRequest.UserName))
            {
                IEnumerable<string> users = new List<string>();
                var ownerGroups = GetGroupsByLevel(2, accessRequest.Org_Id, 0);
                int addRequestResult = 0;
                if (ownerGroups.Count > 0)
                {
                    int groupId = ownerGroups.FirstOrDefault().Group_ID;
                    users = GetGroupUsers(groupId).Select(x => x.User.Email);
                    Entities.UserRequest request = new Entities.UserRequest();
                    request.Org_Id = accessRequest.Org_Id;
                    request.Requested_By = accessRequest.UserName;
                    request.Description = accessRequest.EmailBody;
                    addRequestResult = AddUserRequest(request);
                }
                else if (ownerGroups.Count <= 0 || users.Count() <= 0)
                    users = GetGroupUsers(1).Select(x => x.User.Email);
                string toAddressList = string.Join(",", users.ToArray());
                var ccUser = GetUserInfo(accessRequest.UserName);
                string subject = "Request to add User to " + accessRequest.Org_Name + " User Group.";

                StringBuilder sbBody = new StringBuilder();
                sbBody.AppendLine("Hello, ");
                sbBody.Append("<br/><br/>");
                sbBody.AppendLine("Please provide access to create JSSE for " + accessRequest.Org_Name + " User Group.");
                sbBody.Append("<br/><br/>");
                sbBody.AppendLine(@"<b>Request Sent by: </b>");
                sbBody.Append(ccUser.FullName);
                sbBody.Append("<br/><br/>");
                sbBody.AppendLine(@"<b>Add to: </b>");
                sbBody.Append(accessRequest.Org_Name + " User Group");
                sbBody.Append("<br/><br/>");
                sbBody.AppendLine(@"<b>Message:</b> ");
                sbBody.Append(accessRequest.EmailBody);
                sbBody.Append("<br/><br/>");
                if (addRequestResult == -2)
                {
                    sbBody.Append("User Group does not exist for Selected Organization. Email has been sent to Administrator to Create User Group.");
                    sbBody.Append("<br/><br/>");
                }
                else if (addRequestResult == -1)
                {
                    sbBody.Append("Request had already been created by you earlier, Selected Organization. Email has been sent.");
                    sbBody.Append("<br/><br/>");
                }              
                SendEmailNotification(sbBody.ToString(), toAddressList, ccUser.Email, subject);
                if (addRequestResult == -2)
                    throw new Exception(JSSEConstants.USER_GROUP_NOT_EXIST_ADMIN_EMAILED);
                return 1;
            }
           else
            {
                throw new Exception(JSSEConstants.USER_NAME_CANNOT_BLANK);                
            }
        }
                   
        
        public int AddUserToGroupByRequest(Entities.UserRequest request)
        {
            Entities.UserGroup group = new Entities.UserGroup();
            group.User = request.User;
            group.Group = request.Group;
            //Add User to Group from Request
            try
            {
                JSSESecurityManager.AddUser(group);
            }
            catch (Exception ex)
            {
                //Check Data base Error to see if User Already member of Group, Then Remove Request
                if (ex.Message.IndexOf(JSSEConstants.USER_ALREADY_IN_GROUP) > 0)
                    RemoveUserRequest(request);
                throw ex;
            };
            RemoveUserRequest(request);
            var ccUsers = string.Empty;
            //Get Groups By Level Id (1 for admin 2 for Owner and 3 for user of JSSE)
            var ownerGroups = GetGroupsByLevel(2, request.Group.Org_Id, 0);
            //If Owner Group Exists, then Get Users Owner List in to Variable for Email
            if (ownerGroups.Count > 0)
            {
                int groupId = ownerGroups.FirstOrDefault().Group_ID;
                var ccUserList = GetGroupUsers(groupId).Select(x => x.User.Email);
                ccUsers = string.Join(",", ccUserList.ToArray());
            }
            else
            {
                string userName = request.User.User_ID;
                ccUsers = GetUserInfo(userName).Email;
            }
            //Send Email notification to User and CC Owner
            string subject = string.Format(JSSEConstants.ACCESS_GRANT_TEXT, request.Group.GroupName);// "Access has been GRANTED to " + request.Group.GroupName + " Group.";
            StringBuilder sbBody = new StringBuilder();         
            sbBody.AppendLine(subject);
            sbBody.Append("<br/>");
            sbBody.AppendLine(@"<b>Request Sent by: </b>");
            sbBody.Append(request.User.FullName);
            sbBody.Append("<br/>");
            sbBody.AppendLine(@"<b>Granted Access to: </b>");
            sbBody.Append(request.Group.GroupName + " Group");          
            SendEmailNotification(sbBody.ToString(), request.User.Email, ccUsers, subject);
            return 1;
        }
     
      
        public int RemoveUserRequestEmail(Entities.UserRequest request)
        {
            RemoveUserRequest(request);
            string subject = string.Format(JSSEConstants.ACCESS_DENY_TEXT, request.Group.GroupName);
            var ccUsers = string.Empty;
            var ownerGroups = GetGroupsByLevel(2, request.Group.Org_Id, 0);
            if (ownerGroups.Count > 0)
            {
                int groupId = ownerGroups.FirstOrDefault().Group_ID;
                var ccUserList = GetGroupUsers(groupId).Select(x => x.User.Email);
                ccUsers = string.Join(",", ccUserList.ToArray());
            }
            else
            {
                string userName = request.User.User_ID;
                ccUsers = GetUserInfo(userName).Email;
            }
            StringBuilder sbBody = new StringBuilder();         
            sbBody.AppendLine("Access has been DENIED to JSSE Group: " + request.Group.GroupName + ".");
            sbBody.Append("<br/>");
            sbBody.AppendLine(@"<b>Request Sent by: </b>");
            sbBody.Append(request.User.FullName);
            sbBody.Append("<br/>");
            sbBody.AppendLine(@"<b>Denied Access to: </b>");
            sbBody.Append(request.Group.GroupName + " Group");           
            SendEmailNotification(sbBody.ToString(), request.User.Email, ccUsers, subject);
            return 1;
        }

        public int SendEmailNotification(string body, string toAddressList, string ccList, string subject)
        {
            StringBuilder sbBody = new StringBuilder();
            //sbBody.AppendLine(heading); // later banner heading and logo
            sbBody.Append("<br/>");
            sbBody.Append(@"<table style='width:100%;font-family:calibri;color:#00558D; border-style: solid; border-width:1px; border-color: #bfbfbf; border-collapse: collapse;'> ");
            sbBody.Append(@"<tr><th size='3' style='width:100%; border: 1px solid #bfbfbf; height:33px;background-color:#bfbfbf; font-size:20px;color:#000000;'> <b>EH&S JSSE Mobile Application Email</b></td> ");
            sbBody.Append(@"<th rowSpan='2' style='border: 1px solid #bfbfbf;'></td></tr>");
            sbBody.Append(@"<tr><td style='width:100%;border: 1px solid #558fd5; height:10px;background-color:#558fd5;color:white;'><b>" + subject + "</b></td></tr>");
            sbBody.Append(@"<tr><td colspan='2'>");
            sbBody.AppendLine(@"<div style='font-family:calibri;color:#00558D; border-color: gray'><br/>" + body);
            sbBody.Append("<br/><br/>");         
            sbBody.AppendLine("Thanks ");
            sbBody.Append("<br/>");
            sbBody.AppendLine("EH&S JSSE Mobile Application");
            sbBody.Append("<br/><br/>");
            sbBody.AppendLine("<b>Note:</b> This is a System generated email, please do not reply.");
            sbBody.Append("</td> </tr></table><br/></div>");
            UtilityService.SendMail(sbBody.ToString(), toAddressList, ccList, subject);
            return 1;
        }
    }
}
