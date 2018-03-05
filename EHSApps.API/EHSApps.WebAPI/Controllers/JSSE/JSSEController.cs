using System;
using System.Linq;
using System.Web.Http;
using EHSApps.API.JSSE.Entity;
using EHSApps.API.JSSE.Services;
using System.Web;
using System.Web.Http.Description;
using System.Collections.Generic;
using EHSApps.WebAPI.JSSE.Interfaces;

namespace EHSApps.WebAPI.JSSE.Controllers
{
    [RoutePrefix("api/jsse")]
    public class JSSEController : ApiController
    {
        IJSSEService jsseService;
        public JSSEController()
        {
            jsseService = new JSSEService();
        }
        #region "All GET Service Methods"      
        /// <summary>
        /// Get JSSE information with all categories and behaviors, by JSSE ID
        /// </summary>
        /// <param name="jsseId">jsse Id is numeric</param>
        /// <remarks> Get JSSE information with all categories and behaviors, by JSSE ID</remarks>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns></returns>
        [HttpGet]
        [Route("ByID/{jsseId:int}")]
        [ResponseType(typeof(JSSEMain))]
        public JSSEMain GetJSSEByID(int jsseId)
        {          
            return jsseService.GetJSSE(jsseId);
        }
        /// <summary>
        /// Get regions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("regions")]
        [ResponseType(typeof(Region))]
        public Region[] GetRegions()
        {           
            return jsseService.GetRegions().ToArray();
        }
        /// <summary>
        /// All master categories with behaviors. if Org Id is 0 (zero) Enterprise behaviors only, else gets both enterprise and org specific behaviors. 
        /// </summary>
        /// <param name="org_Id">organization Id of observee</param>
        /// <remarks> All master categories with behaviors. if Org Id is 0 (zero) Enterprise behaviors only, else gets both enterprise and org specific behaviors. </remarks>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns></returns>
        [HttpGet]
        [Route("categories/{org_Id:int}")]
        [ResponseType(typeof(JSSECategory))]
        public JSSECategory[] GetAllMasterCategories(int org_Id)
        {           
            return jsseService.GetAllMasterCategories(org_Id).ToArray();
        }
        /// <summary>
        /// Active master categories with behaviors. if Org Id is 0 (zero) Enterprise behaviors only, else gets both enterprise and org specific behaviors. 
        /// </summary>
        /// <param name="org_Id">organization Id of observee</param>
        /// <remarks>Active master categories with behaviors. if Org Id is 0 (zero) Enterprise behaviors only, else gets both enterprise and org specific behaviors. </remarks>        /// 
        /// <returns></returns>
        [HttpGet]
        [Route("activecategories/{org_Id:int}")]
        [ResponseType(typeof(JSSECategory))]
        public JSSECategory[] GetActiveMasterCategories(int org_Id)
        {            
            return jsseService.GetAllActiveMasterCategories(org_Id).ToArray();
        }

        /// <summary>
        /// Active master categories with behaviors. if Org Id is 0 (zero) Enterprise behaviors only, else gets both enterprise and org specific behaviors. 
        /// </summary>
        /// <param name="org_Id">organization Id of observee</param>
        /// <remarks>Active master categories with behaviors. if Org Id is 0 (zero) Enterprise behaviors only, else gets both enterprise and org specific behaviors. </remarks>        /// 
        /// <returns></returns>
        [HttpGet]
        [Route("activecategories/{userName}")]
        [ResponseType(typeof(JSSECategory))]
        public JSSECategory[] GetActiveMasterCategories(string userName)
        {          
            return jsseService.GetAllActiveMasterCategories(userName).ToArray();
        }
        /// <summary>
        /// Get major groups, orgs , depts and sections based on current user permissions in JSSE system.
        /// </summary>
        /// <param name="userId"></param>
        /// <remarks>Get major groups, orgs , depts and sections based on current user permissions in JSSE system.</remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("majorgroups/{userId}")]
        [ResponseType(typeof(MajorGroup))]
        public MajorGroup[] GetUserMajorGroups(string userId)
        {           
            userId = string.IsNullOrWhiteSpace(userId) ? null : "Coned\\" + userId;
            return jsseService.GetUserMajorGroups(userId, 3).ToArray();
        }
        /// <summary>
        /// Get all major groups
        /// </summary>
        /// <remarks>Get all major groups</remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("majorgroups")]
        [ResponseType(typeof(MajorGroup))]
        public MajorGroup[] GetJSSEMajorGroups()
        {           
            return jsseService.GetUserMajorGroups(string.Empty, 1).ToArray();
        }
        /// <summary>
        /// Get all orgs for selected Major Group. Don't use this for user permitted orgs.
        /// </summary>
        /// <param name="mGroupId">major group Id</param>
        /// <remarks> Get all orgs for selected Major Group. Don't use this for user permitted orgs.</remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("organizations/{mGroupId}")]
        [ResponseType(typeof(Organization))]
        public Organization[] GetOrganizations(string mGroupId)
        {           
            return jsseService.GetOrganizations(mGroupId).ToArray();
        }
        /// <summary>
        /// Get Departments by org id
        /// </summary>
        /// <param name="orgId">organization Id</param>
        /// <remarks>Get Departments by org id</remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("departments/{orgId}")]
        [ResponseType(typeof(Department))]
        public Department[] GetDepartments(string orgId)
        {            
            return jsseService.GetDepartments(orgId).ToArray();
        }
        /// <summary>
        /// Get sections by department id
        /// </summary>
        /// <param name="deptId">department Id</param>
        /// <remarks>Get sections by department id</remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("sections/{deptId}")]
        [ResponseType(typeof(Section))]
        public Section[] GetSections(string deptId)
        {            
            return jsseService.GetSections(deptId).ToArray();
        }
        /// <summary>
        /// Search JSSEs for a selected Organization with from Date , to date. need to pass current user,  ownerOrgIds as comma seperated owner org Ids of current user. group type has to be 2 for Owner and 1 for admin role.
        /// </summary>
        /// <param name="search"></param>
        /// <remarks>Search JSSEs for a selected Organization with from Date , to date. need to pass current user,  ownerOrgIds as comma seperated owner org Ids of current user. group type has to be 2 for Owner and 1 for admin role.</remarks>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ResponseType(typeof(JSSEMain))]
        public JSSEMain[] GetUserOrgJSSEs([FromBody]JSSESearch search)
        {           
            //bool isAnon = jsseType == "anon" ? true : false;
            if (search != null)
            {
                if (search.ToDate != null)
                    search.ToDate = search.ToDate.Replace("AM", "PM").Replace("12:00", "11:59");
                DateTime fromJSSEDate, toJSSEDate;
                DateTime.TryParse(search.FromDate, out fromJSSEDate);
                DateTime.TryParse(search.ToDate, out toJSSEDate);
                //string[] ownerOrgs = search.OwnerOrgIds.Split(',');
                search.Org_Id = search.Org_Id == null ? 0 : search.Org_Id;
                //if (!ownerOrgs.Any(o => o == search.Org_Id.ToString()) && search.GroupType == 2)
                //    search.GroupType = 3;
                //string userId = search.GroupType > 2 ? search.UserName : string.Empty;
                return jsseService.GetUserJSSEs(search).ToArray();
            }
            else
                return new List<JSSEMain>().ToArray();
        }

        #endregion

        #region "All POST Service Methods"

        [HttpPost]
        [Route("addupdate")]
        public int PutJSSE([FromBody]JSSEMain jsse)
        {            
           // jsse.JSSEEnteredBy = HttpContext.Current.User.Identity.Name;
            return jsseService.InsertUpdateJSSE(jsse);
        }

        [HttpPost]
        [Route("updateJSSETemp")]
        public int UpdateJSSETemp([FromBody]JSSEMain jsse)
        {            
            return jsseService.UpdateJSSETemp(jsse, null, null);
        }

        [HttpPost]
        [Route("updateJSSETempImage")]
        public int UpdateJSSETempImage([FromBody]JSSEMain jsse)
        {           
            byte[] image1 = Convert.FromBase64String(jsse.Image);     
            return jsseService.UpdateJSSETemp(jsse, image1, jsse.ImageName);
        }
        #endregion
    }
}
