using System.Web.Http;
using EHSApps.API.JSSE.Entity;
using EHSApps.API.JSSE.Services;
using System.Web.Http.Description;
using EHSApps.WebAPI.JSSE.Interfaces;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights;
using System;
using ConEdison.EHSApps.WebAPI.interfaces;

namespace EHSApps.WebAPI.JSSE.Controllers
{
    [RoutePrefix("api/jsse/security")]
    public class SecurityController : ApiController
    {
        ISecurityService securityService;
        #region "All GET Service Methods"

        public SecurityController()
        {
            securityService = new SecurityService();
        }
        /// <summary>
        /// Send access request to Owners of selected organization
        /// </summary>
        /// <param name="request"> request will have org id, name and  description for email body and current user domain name.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("request")]
        public int SendAccessRequest([FromBody]SecurityRequest request)
        {           
            return securityService.SendRequestEmail(request);
        }

        /// <summary>
        /// Get all Access Requests by Group Id
        /// </summary>
        /// <param name="groupId"> Security Group ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("AccessRequests")]
        [ResponseType(typeof(UserRequest[]))]
        public UserRequest[] GetAccessRequests(int groupId)
        {
            return securityService.GetGroupRequests(groupId).ToArray();
        }
        /// <summary>
        /// Grant Access for a User  access request
        /// </summary>
        /// <param name="request"> user request class with User and Group Info</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GrantAccess")]       
        public int GrantAccess([FromBody]UserRequest request)
        {
            return securityService.AddUserToGroupByRequest(request);
        }
        /// <summary>
        ///  Deny Access for a User  access request
        /// </summary>
        /// <param name="request">user request class with User and Group Info</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DenyAccess")]       
        public int GetAccessRequests([FromBody]UserRequest request)
        {
            return securityService.RemoveUserRequestEmail(request);
        }
        /// <summary>
        /// Get Current user security and groups informations
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>Get Current user security and groups informations</remarks>
        /// <returns></returns>
        [HttpGet]        
        [Route("usersecurity")]
        [ResponseType(typeof(UserGroup))]
        public UserGroup GetUserSecurity(string userName)
        {
            return securityService.GetUserSecurity(userName);
        }

        [HttpGet]
        [Route("searchOrgUsers")]
        [ResponseType(typeof(UserInfo))]
        public UserInfo[] SearchOrgUsers(string firstName ="",string lastName="",int org_id=0)
        {
            IJSSEService jsseService = new JSSEService();
            return jsseService.SearchOrgUsers(firstName, lastName, org_id).ToArray();
        }

        [HttpGet]
        [Route("searchUsers")]
        [ResponseType(typeof(UserInfo))]
        public UserInfo[] SearchUsers(string firstName="", string lastName="")
        {
         
            var telemetry = new RequestTelemetry();
            // Note: A single instance of telemetry client is sufficient to track multiple telemetry items.
            var ai = new TelemetryClient();
            // At start of processing this request:
            // Operation Id and Name are attached to all telemetry and help you identify
            // telemetry associated with one request:
            telemetry.Context.Operation.Id = Guid.NewGuid().ToString();
            telemetry.Context.Operation.Name = "searchUsers Request";

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // ... process the request ...
            IJSSEService jsseService = new JSSEService();
            var result =jsseService.SearchUsers(firstName, lastName).ToArray();

            stopwatch.Stop();
            ai.TrackRequest("requestName", DateTime.UtcNow, stopwatch.Elapsed, "200", true);
            // Response code, success
            return result;
        }
        #endregion

        #region "All POST Service Methods"

        #endregion      
    }
}
