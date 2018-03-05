using System.Collections.Generic;
using System.IO;
using Entities =EHSApps.API.JSSE.Entity;

namespace ConEdison.EHSApps.WebAPI.interfaces
{
    public interface ISecurityService
    {
        Entities.UserGroup GetUserSecurity(string userName);

        Entities.UserInfo GetUserInfo(string userName);

        //int AddUserRequest(Entities.UserRequest request);

        //int RemoveUserRequest(Entities.UserRequest request);

        //int DeActivateUserRequest(Entities.UserRequest request);

        List<Entities.UserGroup> GetGroupUsers(int groupId);

        List<Entities.UserRequest> GetGroupRequests(int groupId);

        List<Entities.Group> GetSecurityGroups(int? levelID);

        List<Entities.Group> GetGroupsByLevel(int levelId, string orgId, int permissionID);

        int SendRequestEmail(Entities.SecurityRequest accessRequest);

        int AddUserToGroupByRequest(Entities.UserRequest request);

        int RemoveUserRequestEmail(Entities.UserRequest request);

        //int SendEmailNotification(string body, string toAddressList, string ccList, string subject);
    }
}
