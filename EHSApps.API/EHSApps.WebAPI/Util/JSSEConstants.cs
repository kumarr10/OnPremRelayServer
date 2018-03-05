using EHSApps.API.JSSE.Data;
using EHSApps.API.JSSE.Entity;
using ConEdison.EHSApps.WebAPI.Models;
using System;
using System.Web;

namespace EHSApps.API.JSSE.Utils
{
    public static class JSSEConstants
    {
        public const string ACCESS_GRANT_TEXT = "Access has been GRANTED to {0} Group.";
        public const string USER_ALREADY_IN_GROUP = "is already a member";
        public const string ACCESS_DENY_TEXT = "Access has been DENIED to {0} Group.";
        public const string USER_GROUP_NOT_EXIST_ADMIN_EMAILED = "User Group does not exist for Selected Organization. An Email has been sent to JSSE Administrator.";
        public const string USER_NAME_CANNOT_BLANK = "User Name should not be blank.";
        public const string REQUEST_TO_ADD_USER = "Request to add User to {0} User Group.";
    }
}
