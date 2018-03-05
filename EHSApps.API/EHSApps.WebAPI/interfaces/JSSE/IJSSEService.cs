using EHSApps.API.JSSE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace EHSApps.WebAPI.JSSE.Interfaces
{
    [ServiceContract(Name ="JSSE")]
    public interface IJSSEService
    {
        [OperationContract(Name="Regions")]
        [WebGet(UriTemplate = "/Regions",
            ResponseFormat = WebMessageFormat.Json)]
        List<Region> GetRegions();

        [OperationContract(Name ="search")]
        [WebGet(UriTemplate = "/search",
              RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]       
        List<JSSEMain> GetUserJSSEs(JSSESearch jsseFilter);

        [OperationContract(Name ="ByID")]
        [WebGet(UriTemplate = "/ByID/{jsseId}",
            ResponseFormat = WebMessageFormat.Json)]
        JSSEMain GetJSSE(int jsseId);

        [OperationContract(Name ="AllCategories")]
        [WebGet(UriTemplate = "/AllCategories/{org_Id}",
            ResponseFormat = WebMessageFormat.Json)]
        List<JSSECategory> GetAllMasterCategories(int? org_Id);

        [OperationContract(Name ="ActiveCategories")]
        [WebGet(UriTemplate = "/ActiveCategories/{userName}",
            ResponseFormat = WebMessageFormat.Json)]
        List<JSSECategory> GetAllActiveMasterCategories(string userName);

        [OperationContract(Name = "ActiveMasterCategories")]
        [WebGet(UriTemplate = "/ActiveMasterCategories/{org_Id}",
        ResponseFormat = WebMessageFormat.Json)]
        List<JSSECategory> GetAllActiveMasterCategories(int? org_Id);

        [OperationContract(Name ="Organizations")]
        [WebGet(UriTemplate = "/Organizations/{majorGroupId}",
            ResponseFormat = WebMessageFormat.Json)]
        List<Organization> GetOrganizations(string majorGroupId);

        [OperationContract(Name ="Departments")]
        [WebGet(UriTemplate = "/Departments/{orgId}",
            ResponseFormat = WebMessageFormat.Json)]
        List<Department> GetDepartments(string orgId);

        [OperationContract(Name ="Sections")]
        [WebGet(UriTemplate = "/Sections/{deptId}",
            ResponseFormat = WebMessageFormat.Json)]
        List<Section> GetSections(string deptId);

        [OperationContract(Name ="MajorGroups")]
        [WebGet(UriTemplate = "/MajorGroups",
            ResponseFormat = WebMessageFormat.Json)]
        List<MajorGroup> GetMajorGroups();

        [OperationContract(Name = "UserOrgs")]
        [WebGet(UriTemplate = "/UserOrgs",
            ResponseFormat = WebMessageFormat.Json)]
        List<Organization> GetUserOrgs(string userId, int groupType);

        [OperationContract(Name = "UserMajorGroups")]
        [WebGet(UriTemplate = "/UserMajorGroups",
            ResponseFormat = WebMessageFormat.Json)]
        List<MajorGroup> GetUserMajorGroups(string userId, int groupType);

        [OperationContract(Name = "GetUserOrgsByPermission")]
        [WebGet(UriTemplate = "/GetUserOrgsByPermission",
            ResponseFormat = WebMessageFormat.Json)]
        List<Organization> GetUserOrgsByPermission(string userId, int groupType, int permissionID);   
            
        [OperationContract(Name ="AddUpdateJSSE")]
        [WebInvoke(UriTemplate = "/AddUpdateJSSE",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        int InsertUpdateJSSE(JSSEMain jsse);

        [OperationContract(Name = "SearchOrgUsers")]
        [WebInvoke(UriTemplate = "/SearchOrgUsers",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        List<UserInfo> SearchOrgUsers(string firstName, string lastName, int org_id);

        [OperationContract(Name = "SearchUsers")]
        [WebInvoke(UriTemplate = "/SearchUsers",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        List<UserInfo> SearchUsers(string firstName, string lastName);

        int UpdateJSSETemp(JSSEMain jsse, byte[] image, string imageName);
    }

    interface IJSSEChannel : IJSSEService, IClientChannel
    {
    }
}
